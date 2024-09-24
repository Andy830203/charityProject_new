using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using charity.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using charity.ViewModels;

namespace charity.Controllers
{
    public class ProductsController : Controller
    {
        private readonly CharityContext _context;

        public ProductsController(CharityContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            //var charityContext = _context.Products.Include(p => p.SellerNavigation);

            //return View(charityContext);
            var products = await _context.Products.Include(p => p.ProductImgs) // 確保載入與產品相關的照片
        .Select(p => new ProductImgViewModel {
            product = p,
            // 顯示第一張照片
            productImgs = p.ProductImgs.Select(img => img.ImgName).ToList()
        })
        .ToListAsync();

        return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.SellerNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            // 查詢產品的圖片路徑
            var images = _context.ProductImgs.Where(pi => pi.PId == id).Select(pi => pi.ImgName).ToList();

            // 創建 ViewModel 並填充資料
            var viewModel = new ProductImgViewModel {
                product = product,
                productImgs = images
            };


            return View(viewModel);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["Seller"] = new SelectList(_context.Members, "Id", "Id");
            ViewData["CategoryList"] = new SelectList(_context.ProductCategories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductImgViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vm.product);
                await _context.SaveChangesAsync();
                // 處理圖片上傳
                if (vm.UploadedImages != null && vm.UploadedImages.Count > 0) {
                    foreach (var image in vm.UploadedImages) {
                        var fileName = Path.GetFileName(image.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products", fileName);

                        // 儲存圖片檔案
                        using (var stream = new FileStream(filePath, FileMode.Create)) {
                            await image.CopyToAsync(stream);
                        }

                        // 將圖片路徑存到資料庫
                        var productImg = new ProductImg {
                            ImgName = "/images/products/" + fileName,
                            PId = vm.product.Id
                        };
                        vm.productImgs.Add("/images/products/" + fileName);
                        _context.ProductImgs.Add(productImg);
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            else {
                foreach (var error in ModelState) {
                    Console.WriteLine($"Key: {error.Key}, Errors: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }
            ViewData["Seller"] = new SelectList(_context.Members, "Id", "Id", vm.product.Seller);
            ViewData["CategoryList"] = new SelectList(_context.ProductCategories, "Id", "Name");
            return View(vm);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currProduct = await _context.Products.Include(p => p.ProductImgs).FirstOrDefaultAsync(p => p.Id == id);
            if (currProduct == null)
            {
                return NotFound();
            }
            var vm = new ProductImgViewModel {
                product = currProduct,
                ExistingImages = currProduct.ProductImgs.Select(img => new UploadedImage {
                    ImgName = img.ImgName,
                    ImgId = img.Id
                }).ToList(),
                UploadedImages = new List<IFormFile>()
            };
            ViewData["Seller"] = new SelectList(_context.Members, "Id", "Id", currProduct.Seller);
            ViewData["CategoryList"] = new SelectList(_context.ProductCategories, "Id", "Name", currProduct.Category);
            return View(vm);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int[] DeleteImages, ProductImgViewModel vm)
        {
            // [Bind("product.Id,product.Name,product.Seller,product.Category,product.price,product.OnShelf,product.OnShelfTime,product.Description,product.Instock,UploadedImages, ExistingImages")] 移除，因為Post功能會異常
            if (id != vm.product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vm.product);
                    await _context.SaveChangesAsync();

                    // 刪除選中的圖片
                    if (DeleteImages.Length > 0) {
                        var imagesToDelete = _context.ProductImgs.Where(pi => DeleteImages.Contains(pi.Id)).ToList();
                        _context.ProductImgs.RemoveRange(imagesToDelete);
                        await _context.SaveChangesAsync();
                    }
                    if (vm.UploadedImages != null && vm.UploadedImages.Count > 0) {
                        foreach (var image in vm.UploadedImages) {
                            var fileName = Path.GetFileName(image.FileName);
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products", fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create)) {
                                await image.CopyToAsync(stream);
                            }

                            var productImg = new ProductImg {
                                ImgName = "/images/products/" + fileName,
                                PId = vm.product.Id
                            };
                            _context.ProductImgs.Add(productImg);
                        }
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(vm.product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Seller"] = new SelectList(_context.Members, "Id", "Id", vm.product.Seller);
            ViewData["CategoryList"] = new SelectList(_context.ProductCategories, "Id", "Name");
            return View(vm);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.SellerNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            // 查詢產品的圖片路徑
            var images = _context.ProductImgs.Where(pi => pi.PId == id).Select(pi => pi.ImgName).ToList();

            // 創建 ViewModel 並填充資料
            var viewModel = new ProductImgViewModel {
                product = product,
                productImgs = images
            };


            return View(viewModel);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
