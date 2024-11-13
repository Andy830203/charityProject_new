using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
                        : "/images/non-found.jpg"  // 如果沒有圖片
                });
        }

        [HttpGet("seller/{id}")]
        public async Task<IEnumerable<IndexProductDTO>> GetProductBySeller(int id) {
            var products = await _context.Products
                .Where(p => p.Seller == id && p.OnShelf == true) // 查詢特定賣家ID目前已上架的商品
                .Include(p => p.ProductImgs) // 包含產品圖片資料
                .Include(p => p.SellerNavigation) // 包含賣家資料
                .Include(p => p.CategoryNavigation) // 包含類別資料
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
                        : "/images/non-found.jpg"  // 如果沒有圖片
                })
                .ToListAsync();

            return products;
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
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products
            .Include(p => p.ProductImgs)  // 包含產品圖片資料
            .Include(p => p.SellerNavigation)  // 包含賣家資料
            .Include(p => p.CategoryNavigation)  // 包含類別資料    
            .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // 查找符合此產品 ID 的所有圖片資料
            var currProductImgs = await _context.ProductImgs
                .Where(img => img.PId == id)
                .Select(img => img.ImgName) // 選取 imgPath 欄位
                .ToListAsync();

            var productDTO = new ProductDetailDTO {
                Id = product.Id,
                Name = product.Name,
                Seller = product.Seller,
                SellerName = product.SellerNavigation != null ? product.SellerNavigation.RealName : "無賣家",
                Price = product.Price,
                Category = product.Category,
                CategoryName = product.CategoryNavigation != null ? product.CategoryNavigation.Name : "無類別",
                Description = product.Description,
                OnShelfTime = product.OnShelfTime,
                Instock = product.Instock,
                imgUrls = currProductImgs
            };
            return Ok(productDTO);
        }
        // PUT: api/Products/Offshelf/5
        [HttpPut("Offshelf/{id}")]
        public async Task<IActionResult> OffshelfProduct(int id) {
            // Retrieve the product by ID
            var product = await _context.Products.FindAsync(id);
            if (product == null) {
                // Return a 404 if the product does not exist
                return NotFound(new { message = "Product not found" });
            }
            product.OnShelf = false;
            try {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) {
                // Handle any errors
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error deactivating product", details = ex.Message });
            }

            // Return a success response
            return Ok(new { message = "Product deactivated successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductsEditDTO productDto, [FromForm] List<IFormFile> images, [FromForm] string? existingImageUrls) {
            // Find the product by ID
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) {
                return NotFound("商品不存在");
            }

            // Update product fields
            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Instock = productDto.Stock;
            product.Description = productDto.Description;
            product.Category = productDto.CategoryId;

            // Parse the existingImageUrls to retain from frontend
            var retainedImageUrls = string.IsNullOrEmpty(existingImageUrls) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(existingImageUrls);
            var imagesToRemove = product.ProductImgs.Where(img => img.PId == id && !retainedImageUrls.Contains(img.ImgName));
            foreach (var img in imagesToRemove) {
                _context.ProductImgs.Remove(img);
            }
            // Add new images
            if (images != null && images.Count > 0) {
                var projectRoot = Directory.GetCurrentDirectory();
                var directoryPath = Path.Combine(projectRoot, "..", "charity", "wwwroot", "images", "products");
                if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

                foreach (var image in images) {
                    var fileName = $"{Guid.NewGuid()}_{image.FileName}";
                    var filePath = Path.Combine(directoryPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create)) {
                        await image.CopyToAsync(stream);
                    }
                    _context.ProductImgs.Add(new ProductImg { ImgName = $"/images/products/{fileName}", PId = product.Id });
                }
            }
            // _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "商品更新成功" });
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

        // PUT: api/Products/SubtractStock
        [HttpPut("SubtractStock")]
        public async Task<IActionResult> SubtractStock([FromBody] SubStockDTO ssDTO) {
            // Check if the product exists in the database
            var product = await _context.Products.FindAsync(ssDTO.pId);

            if (product == null) {
                return NotFound($"Product with ID {ssDTO.pId} not found.");
            }

            // Check if there's enough stock to subtract
            if (product.Instock < ssDTO.quantity) {
                return BadRequest("Insufficient stock.");
            }

            // Subtract the quantity from the product's stock
            product.Instock -= ssDTO.quantity;

            // Save changes to the database
            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) {
                // Handle database update errors if needed
                return StatusCode(500, "An error occurred while updating the stock.");
            }

            return Ok(new { Message = "Stock updated successfully.", productId = product.Id, newStock = product.Instock });
        }


        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
