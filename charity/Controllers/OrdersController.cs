using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using charity.Models;
using charity.ViewModels;

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
            ViewData["ProductList"] = new SelectList(_context.Products, "Id", "Name"); // Id 顯示商品名稱以便辨識

            // 返回 Partial View
            return PartialView("_CreateOrderPartial", model);
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderViewModel model) {
            if (ModelState.IsValid) {
                // 創建新訂單實體
                var order = new Order {
                    Buyer = model.BuyerId,
                    DiscountCode = model.DiscountCode,
                    OrderTime = DateTime.Now // 設定訂單日期
                };

                // 添加訂單到上下文
                _context.Orders.Add(order);
                await _context.SaveChangesAsync(); // 儲存以獲取訂單 ID

                // 處理訂單項目
                foreach (var item in model.OrderItems) {
                    var orderItem = new OrderItem {
                        OId = order.Id,
                        PId = item.ProductId,
                        Quantity = item.Quantity
                    };
                    _context.OrderItems.Add(orderItem);
                }

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
    
    // GET: Orders/Details/5
    //public async Task<IActionResult> Details(int? id)
    //{
    //    if (id == null)
    //    {
    //        return NotFound();
    //    }

    //    var order = await _context.Orders
    //        .Include(o => o.BuyerNavigation)
    //        .Include(o => o.StatusNavigation)
    //        .Include(o => o.OrderItems)
    //        .FirstOrDefaultAsync(m => m.Id == id);
    //    if (order == null)
    //    {
    //        return NotFound();
    //    }

    //    return View(order);
    //}

    //// GET: Orders/Create
    //public IActionResult Create()
    //{
    //    ViewData["Buyer"] = new SelectList(_context.Members, "Id", "Id");
    //    ViewData["Status"] = new SelectList(_context.OrderStatuses, "Id", "Id");
    //    return View();
    //}

    //// POST: Orders/Create
    //// To protect from overposting attacks, enable the specific properties you want to bind to.
    //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Create([Bind("Id,Buyer,TotalPrice,Status,OrderTime,DiscountCode")] Order order)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        _context.Add(order);
    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }
    //    ViewData["Buyer"] = new SelectList(_context.Members, "Id", "Id", order.Buyer);
    //    ViewData["Status"] = new SelectList(_context.OrderStatuses, "Id", "Id", order.Status);
    //    return View(order);
    //}

    //// GET: Orders/Edit/5
    //public async Task<IActionResult> Edit(int? id)
    //{
    //    if (id == null)
    //    {
    //        return NotFound();
    //    }

    //    var order = await _context.Orders.FindAsync(id);
    //    if (order == null)
    //    {
    //        return NotFound();
    //    }
    //    ViewData["Buyer"] = new SelectList(_context.Members, "Id", "Id", order.Buyer);
    //    ViewData["Status"] = new SelectList(_context.OrderStatuses, "Id", "Id", order.Status);
    //    return View(order);
    //}

    //// POST: Orders/Edit/5
    //// To protect from overposting attacks, enable the specific properties you want to bind to.
    //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Edit(int id, [Bind("Id,Buyer,TotalPrice,Status,OrderTime,DiscountCode")] Order order)
    //{
    //    if (id != order.Id)
    //    {
    //        return NotFound();
    //    }

    //    if (ModelState.IsValid)
    //    {
    //        try
    //        {
    //            _context.Update(order);
    //            await _context.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!OrderExists(order.Id))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }
    //        return RedirectToAction(nameof(Index));
    //    }
    //    ViewData["Buyer"] = new SelectList(_context.Members, "Id", "Id", order.Buyer);
    //    ViewData["Status"] = new SelectList(_context.OrderStatuses, "Id", "Id", order.Status);
    //    return View(order);
    //}

    //// GET: Orders/Delete/5
    //public async Task<IActionResult> Delete(int? id)
    //{
    //    if (id == null)
    //    {
    //        return NotFound();
    //    }

    //    var order = await _context.Orders
    //        .Include(o => o.BuyerNavigation)
    //        .Include(o => o.StatusNavigation)
    //        .FirstOrDefaultAsync(m => m.Id == id);
    //    if (order == null)
    //    {
    //        return NotFound();
    //    }

    //    return View(order);
    //}

    //// POST: Orders/Delete/5
    //[HttpPost, ActionName("Delete")]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> DeleteConfirmed(int id)
    //{
    //    var order = await _context.Orders.FindAsync(id);
    //    if (order != null)
    //    {
    //        _context.Orders.Remove(order);
    //    }

    //    await _context.SaveChangesAsync();
    //    return RedirectToAction(nameof(Index));
    //}

    //private bool OrderExists(int id)
    //{
    //    return _context.Orders.Any(e => e.Id == id);
    //}
}
}
