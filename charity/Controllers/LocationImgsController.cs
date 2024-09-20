using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using charity.Models;

namespace charity.Controllers
{
    public class LocationImgsController : Controller
    {
        private readonly CharityContext _context;

        public LocationImgsController(CharityContext context)
        {
            _context = context;
        }

        // GET: LocationImgs
        public async Task<IActionResult> Index()
        {
            var charityContext = _context.LocationImgs.Include(l => l.LIdNavigation);
            return View(await charityContext.ToListAsync());
        }

        // GET: LocationImgs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationImg = await _context.LocationImgs
                .Include(l => l.LIdNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (locationImg == null)
            {
                return NotFound();
            }

            return View(locationImg);
        }

        // GET: LocationImgs/Create
        public IActionResult Create()
        {
            ViewData["LId"] = new SelectList(_context.Locations, "Id", "Id");
            return View();
        }

        // POST: LocationImgs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LId,ImgName")] LocationImg locationImg)
        {
            if (ModelState.IsValid)
            {
                _context.Add(locationImg);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LId"] = new SelectList(_context.Locations, "Id", "Id", locationImg.LId);
            return View(locationImg);
        }

        // GET: LocationImgs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationImg = await _context.LocationImgs.FindAsync(id);
            if (locationImg == null)
            {
                return NotFound();
            }
            ViewData["LId"] = new SelectList(_context.Locations, "Id", "Id", locationImg.LId);
            return View(locationImg);
        }

        // POST: LocationImgs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LId,ImgName")] LocationImg locationImg)
        {
            if (id != locationImg.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(locationImg);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationImgExists(locationImg.Id))
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
            ViewData["LId"] = new SelectList(_context.Locations, "Id", "Id", locationImg.LId);
            return View(locationImg);
        }

        // GET: LocationImgs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationImg = await _context.LocationImgs
                .Include(l => l.LIdNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (locationImg == null)
            {
                return NotFound();
            }

            return View(locationImg);
        }

        // POST: LocationImgs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var locationImg = await _context.LocationImgs.FindAsync(id);
            if (locationImg != null)
            {
                _context.LocationImgs.Remove(locationImg);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationImgExists(int id)
        {
            return _context.LocationImgs.Any(e => e.Id == id);
        }
    }
}
