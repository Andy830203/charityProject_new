﻿using System;
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
            var images = _context.ProductImgs.Where(pi => pi.Id == id).Select(pi => pi.ImgName).ToList();

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
        public async Task<IActionResult> Create([Bind("Name,Seller,Category,Price,OnShelf,OnShelfTime,Description,Instock")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Seller"] = new SelectList(_context.Members, "Id", "Id", product.Seller);
            ViewData["CategoryList"] = new SelectList(_context.ProductCategories, "Id", "Name");
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["Seller"] = new SelectList(_context.Members, "Id", "Id", product.Seller);
            ViewData["CategoryList"] = new SelectList(_context.ProductCategories, "Id", "Name");
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Seller,Category,Price,OnShelf,OnShelfTime,Description,Instock")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["Seller"] = new SelectList(_context.Members, "Id", "Id", product.Seller);
            return View(product);
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

            return View(product);
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
