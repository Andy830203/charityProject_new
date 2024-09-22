using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using charity.Models;
using Microsoft.Extensions.Hosting;
using charity.ViewModels;

namespace charity.Controllers
{
    public class LocationsController : Controller
    {
        private readonly CharityContext _context;
        public LocationsController(CharityContext context, IWebHostEnvironment environment)
        {
            _context = context;
        }

        // GET: Locations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Locations.ToListAsync());
        }

        // GET: Locations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: Locations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Longitude,Latitude,Description,Address,PlusCode,Capacity")] Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: Locations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Longitude,Latitude,Description,Address,PlusCode,Capacity")] Location location)
        {
            if (id != location.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.Id))
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
            return View(location);
        }

        // GET: Locations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location != null)
            {
                _context.Locations.Remove(location);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.Id == id);
        }
        // 圖片上傳動作
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            //檢查是否image有值(使否有上傳)
            if (image != null && image.Length > 0)
            {
                // 檢查是否是圖片
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                //GetExtension取得副檔名路徑、ToLower轉小寫
                var extension = Path.GetExtension(image.FileName).ToLower();
                //假設副檔名不符合
                if (Array.IndexOf(allowedExtensions, extension) < 0)
                {
                    //回傳Jsonsuccess:
                    //success = false告知結果失敗
                    return Json(new { success = false, message = "僅允許上傳圖片檔案 (jpg, jpeg, png, gif)" });
                }
                //Directory.GetCurrentDirectory()當前專案根目錄
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/", "test");
                // 確保 images 資料夾存在，否則創建
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                
                var filePath = Path.Combine(uploadsFolder, image.FileName);

                // 將圖片寫入 wwwroot/test
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                return Json(new { success = true, message = "圖片上傳成功!", imageUrl = "/images/" + image.FileName });
            }

            return Json(new { success = false, message = "請選擇一個圖片檔案。" });
        }
    }
}
