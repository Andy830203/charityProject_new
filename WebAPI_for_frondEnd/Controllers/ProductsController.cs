using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_for_frondEnd.DTO;
using WebAPI_for_frondEnd.Models;

namespace WebAPI_for_frondEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly charityContext _context;

        public ProductsController(charityContext context)
        {
            _context = context;
        }

        // testing, can get all data successfully
        // GET: api/Products
        [HttpGet]
        public async Task<IEnumerable<IndexProductDTO>> GetProducts() {
            return _context.Products
                .Include(p => p.ProductImgs)  // 包含產品圖片資料
                .Include(p => p.SellerNavigation)  // 包含賣家資料
                .Include(p => p.CategoryNavigation)  // 包含類別資料
                .Select(p => new IndexProductDTO {
                    Id = p.Id,
                    Name = p.Name,
                    SellerId = p.Seller,
                    SellerName = p.SellerNavigation != null ? p.SellerNavigation.RealName : "無賣家",
                    Category = p.Category,
                    CategoryName = p.CategoryNavigation != null ? p.CategoryNavigation.Name : "無類別",
                    Price = p.Price,
                    OnShelfTime = p.OnShelfTime,
                    Description = p.Description,
                    Instock = p.Instock,
                    MainImageUrl = p.ProductImgs != null && p.ProductImgs.Any()
                        ? p.ProductImgs.FirstOrDefault().ImgName  // 使用第一張圖片作為主圖片
                        : "NoPicture"  // 如果沒有圖片
                });
        }

        // GET: api/Products/maxPrice
        [HttpGet("maxPrice")]
        public IActionResult GetMaxPrice() {
            // 檢查產品表是否有商品
            if (!_context.Products.Any(p => p.OnShelf == true)) {
                // 若沒有上架中的商品，返回404狀態和相應訊息
                return NotFound(new { message = "No products available." });
            }

            // 獲取最大價格
            var maxPrice = _context.Products
                .Where(p => p.OnShelf == true) // 僅考慮上架中的產品
                .Max(p => p.Price); // 獲取最大價格

            return Ok(new { maxPrice });
        }

        // get products data based on parameters from frontend
        [HttpPost("search")]
        public async Task<IActionResult> SearchProducts([FromBody] ProductQueryDTO query) {
            var productsQuery = _context.Products
            .Include(p => p.ProductImgs)  // 包含產品圖片資料
            .Include(p => p.SellerNavigation)  // 包含賣家資料
            .Include(p => p.CategoryNavigation)  // 包含類別資料
            .Where(p => string.IsNullOrEmpty(query.Keyword) || p.Name.Contains(query.Keyword)) // 搜尋關鍵字篩選
            .Where(p => query.CategoryId == 0 || p.Category == query.CategoryId) // 類別篩選
            .Where(p => p.OnShelf == true); // 必須為上架中

            if (query.PriceThreshold != 0) {
                productsQuery = productsQuery.Where(p => p.Price >= query.PriceThreshold);
            }
            // 根據 sortBy 屬性設置排序
            switch (query.SortBy) {
                case "priceAsc":
                    productsQuery = productsQuery.OrderBy(p => p.Price);
                    break;
                case "priceDesc":
                    productsQuery = productsQuery.OrderByDescending(p => p.Price);
                    break;
                case "timeAsc":
                    productsQuery = productsQuery.OrderBy(p => p.OnShelfTime);
                    break;
                case "timeDesc":
                    productsQuery = productsQuery.OrderByDescending(p => p.OnShelfTime);
                    break;
                default:
                    productsQuery = productsQuery.OrderBy(p => p.Name); // 預設按名稱排序
                    break;
            }

            // 總數量
            var totalItems = await productsQuery.CountAsync();

            // 分頁處理
            var products = productsQuery
                .Skip((query.Page - 1) * 9) // the page size is fixed to 9
                .Take(9)
                .Select(p => new IndexProductDTO {
                    Id = p.Id,
                    Name = p.Name,
                    SellerName = p.SellerNavigation != null ? p.SellerNavigation.RealName : "無賣家",
                    Category = p.Category,
                    CategoryName = p.CategoryNavigation != null ? p.CategoryNavigation.Name : "無類別",
                    Price = p.Price,
                    OnShelfTime = p.OnShelfTime,
                    Description = p.Description,
                    Instock = p.Instock,
                    MainImageUrl = p.ProductImgs != null && p.ProductImgs.Any()
                        ? p.ProductImgs.FirstOrDefault().ImgName  // 使用第一張圖片作為主圖片
                        : "NoPicture"  // 如果沒有圖片
                });

            // 計算總頁數
            int totalPages = (int)Math.Ceiling(totalItems / (double)9);

            // 返回結果
            return Ok(new {
                totalPages = totalPages,
                productsResult = products
            });
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
