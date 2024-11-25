using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using charity.Models;
using charity.ViewModels;
using Newtonsoft.Json;

namespace charity.Controllers
{
    public class OrdersController : Controller
    {
        private readonly CharityContext _context;

        public OrdersController(CharityContext context)
        {
            _context = context;
        }

        // GET: Orders
        public IActionResult Index() {
            return View();
        }

        public async Task<JsonResult> IndexJson() {
            var orders = await _context.Orders
                .Include(o => o.StatusNavigation)
                .Include(o => o.BuyerNavigation)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.PIdNavigation) // 如果需要產品名稱等
                .Select(o => new OrderViewModel {
                    Id = o.Id,
                    Buyer = o.Buyer,
                    TotalPrice = o.TotalPrice,
                    Status = o.Status,
                    OrderTime = o.OrderTime,
                    StatusName = o.StatusNavigation!= null ? o.StatusNavigation.Name : "無狀態",
                    OrderItems = o.OrderItems.Select(oi => new OrderItemViewModel {
                        Id = oi.Id,
                        PId = oi.PId,
                        PName = oi.PIdNavigation != null ? oi.PIdNavigation.Name: "無商品名",
                        Quantity = oi.Quantity,
                        ShippedTime = oi.ShippedTime,
                        Score = oi.Score,
                    }).ToList()
                }).ToListAsync();

            return Json(new { data = orders });
        }
        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id) {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.PIdNavigation) // 加載產品資訊
                .Include(o => o.BuyerNavigation) // 加載購買者資訊
                .Include(o => o.StatusNavigation) // 加載狀態資訊
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) {
                return NotFound();
            }
            var viewModel = new OrderDetailsViewModel {
                OrderId = order.Id,
                BuyerId = order.Buyer,
                BuyerName = order.BuyerNavigation?.RealName,
                TotalPrice = order.TotalPrice,
                Status = order.StatusNavigation.Name,
                OrderTime = order.OrderTime,
                Items = order.OrderItems.Select(oi => new OrderItemForDetailsViewModel {
                    ProductId = oi.PId,
                    ProductName = oi.PIdNavigation?.Name,
                    Quantity = oi.Quantity,
                    Price = oi.PIdNavigation?.Price,
                    Subtotal = (oi.Quantity ?? 0) * (oi.PIdNavigation?.Price ?? 0)
                }).ToList()
            };
            return PartialView("_OrderDetails", viewModel);
        }

        [HttpPost]
        public IActionResult Delete(int id) {
            // 確認訂單是否存在
            var order = _context.Orders
                                .Include(o => o.OrderItems)
                                .FirstOrDefault(o => o.Id == id);

            if (order == null) {
                return Json(new { success = false, message = "訂單不存在！" });
            }

            try {
                // 刪除訂單明細
                _context.OrderItems.RemoveRange(order.OrderItems);

                // 刪除訂單
                _context.Orders.Remove(order);

                // 保存變更
                _context.SaveChanges();

                return Json(new { success = true, message = "訂單已刪除成功！" });
            }
            catch (Exception ex) {
                return Json(new { success = false, message = "刪除失敗：" + ex.Message });
            }
        }

        // GET: Orders/Create
        public IActionResult Create() {
            var model = new CreateOrderViewModel {
            };
            // 加載買家清單
            ViewData["BuyerList"] = new SelectList(_context.Members, "Id", "Id");

            // 加載商品清單
            ViewData["ProductList"] = new SelectList(_context.Products, "Id", "Id"); // Id 顯示商品名稱以便辨識

            // 返回 Partial View
            return PartialView("_CreateOrderPartial", model);
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderViewModel model, string OrderItemsJson) {
            if (!string.IsNullOrEmpty(OrderItemsJson)) {
                model.OrderItems = JsonConvert.DeserializeObject<List<CreateOrderItemViewModel>>(OrderItemsJson);
            }
            if (ModelState.IsValid) {
                // 創建新訂單實體
                var order = new Order {
                    Buyer = model.BuyerId,
                    DiscountCode = model.DiscountCode,
                    Status = 1,
                    OrderTime = DateTime.Now // 設定訂單日期
                };

                // 添加訂單到上下文
                _context.Orders.Add(order);
                await _context.SaveChangesAsync(); // 儲存以獲取訂單 ID

                int totalPrice = 0;
                // 處理訂單項目
                foreach (var item in model.OrderItems) {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                    if (product != null) {
                        if (product.Price != null && item.Quantity != null) {
                            int? subTotal = product.Price * item.Quantity;
                            totalPrice += (int)subTotal;
                            var orderItem = new OrderItem {
                                OId = order.Id,
                                PId = item.ProductId,
                                Quantity = item.Quantity
                            };
                            _context.OrderItems.Add(orderItem);
                        }
                    }
                }
                // 更新訂單總金額
                order.TotalPrice = totalPrice;
                // 儲存訂單項目
                await _context.SaveChangesAsync();

                // 重定向或返回成功回應
                return Json(new { success = true, message = "訂單已成功建立。" });
            }

            // 如果模型無效，返回部分視圖以進行錯誤處理
            ViewData["BuyerList"] = new SelectList(_context.Members, "Id", "Id", model.BuyerId);
            ViewData["ProductList"] = new SelectList(_context.Products, "Id", "Name");
            return PartialView("_CreateOrderPartial", model);
        }
    

    }
}
