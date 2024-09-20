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
    public class EventImgsController : Controller
    {
        private readonly CharityContext _context;

        public EventImgsController(CharityContext context)
        {
            _context = context;
        }

        // GET: EventImgs
        public async Task<IActionResult> Index()
        {
            var charityContext = _context.EventImgs.Include(e => e.EIdNavigation);
            return View(await charityContext.ToListAsync());
        }

        // GET: EventImgs/Details/5
        public async Task<IActionResult> Details(int? id, string? fromList)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventImg = await _context.EventImgs
                .Include(e => e.EIdNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventImg == null)
            {
                return NotFound();
            }

            if (!Url.IsLocalUrl(fromList))
            {
                fromList = Url.Action("Index", "EventImgs");
            }
            ViewBag.FromList = fromList;
            return View(eventImg);
        }

        // GET: EventImgs/Create
        public IActionResult Create(string? fromList)
        {
            if (!Url.IsLocalUrl(fromList))
            {
                fromList = Url.Action("Index", "EventImgs");
            }
            ViewBag.FromList = fromList;
            ViewData["EId"] = new SelectList(_context.Events, "Id", "Id");
            return View();
        }

        // POST: EventImgs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string? fromList, [Bind("Id,EId,ImgName")] EventImg eventImg)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventImg);
                await _context.SaveChangesAsync();
                if (!string.IsNullOrEmpty(fromList))
                {
                    return RedirectToAction("Index", fromList);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EId"] = new SelectList(_context.Events, "Id", "Id", eventImg.EId);

            if (!string.IsNullOrEmpty(fromList))
            {
                return View(fromList);
            }
            return View(eventImg);
        }

        // GET: EventImgs/Edit/5
        public async Task<IActionResult> Edit(int? id, string? fromList)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventImg = await _context.EventImgs.FindAsync(id);
            if (eventImg == null)
            {
                return NotFound();
            }

            if (!Url.IsLocalUrl(fromList))
            {
                fromList = Url.Action("Index", "EventImgs");
            }
            ViewBag.FromList = fromList;
            ViewData["EId"] = new SelectList(_context.Events, "Id", "Id", eventImg.EId);
            return View(eventImg);
        }

        // POST: EventImgs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string? fromList, [Bind("Id,EId,ImgName")] EventImg eventImg)
        {
            if (id != eventImg.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventImg);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventImgExists(eventImg.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (!string.IsNullOrEmpty(fromList))
                {
                    return RedirectToAction("Index", fromList);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EId"] = new SelectList(_context.Events, "Id", "Id", eventImg.EId);
            if (!string.IsNullOrEmpty(fromList))
            {
                return RedirectToAction("Index", fromList);
            }
            return View(eventImg);
        }

        // GET: EventImgs/Delete/5
        public async Task<IActionResult> Delete(int? id, string fromList)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventImg = await _context.EventImgs
                .Include(e => e.EIdNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventImg == null)
            {
                return NotFound();
            }

            if (!Url.IsLocalUrl(fromList))
            {
                fromList = Url.Action("Index", "EventImgs");
            }
            ViewBag.FromList = fromList;
            return View(eventImg);
        }

        // POST: EventImgs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string? fromList)
        {
            var eventImg = await _context.EventImgs.FindAsync(id);
            if (eventImg != null)
            {
                _context.EventImgs.Remove(eventImg);
            }

            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(fromList))
            {
                return RedirectToAction("Index", fromList);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EventImgExists(int id)
        {
            return _context.EventImgs.Any(e => e.Id == id);
        }
    }
}
