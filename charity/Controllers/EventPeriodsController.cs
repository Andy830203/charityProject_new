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
    public class EventPeriodsController : Controller
    {
        private readonly CharityContext _context;

        public EventPeriodsController(CharityContext context)
        {
            _context = context;
        }

        // GET: EventPeriods
        public async Task<IActionResult> Index()
        {
            var charityContext = _context.EventPeriods.Include(e => e.EIdNavigation);
            return View(await charityContext.ToListAsync());
        }

        // GET: EventPeriods/Details/5
        public async Task<IActionResult> Details(int? id, string fromList)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventPeriod = await _context.EventPeriods
                .Include(e => e.EIdNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventPeriod == null)
            {
                return NotFound();
            }

            if (!Url.IsLocalUrl(fromList))
            {
                fromList = Url.Action("Index", "EventPeriods");
            }
            ViewBag.FromList = fromList;
            return View(eventPeriod);
        }

        // GET: EventPeriods/Create
        public IActionResult Create(string? fromList)
        {
            if (!Url.IsLocalUrl(fromList))
            {
                fromList = Url.Action("Index", "EventPeriods");
            }
            ViewBag.FromList = fromList;
            ViewData["EId"] = new SelectList(_context.Events, "Id", "Id");
            return View();
        }

        // POST: EventPeriods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EId,StartTime,EndTime,Description")] EventPeriod eventPeriod)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventPeriod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EId"] = new SelectList(_context.Events, "Id", "Id", eventPeriod.EId);
            return View(eventPeriod);
        }

        // GET: EventPeriods/Edit/5
        public async Task<IActionResult> Edit(int? id, string fromList)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventPeriod = await _context.EventPeriods.FindAsync(id);
            if (eventPeriod == null)
            {
                return NotFound();
            }

            if (!Url.IsLocalUrl(fromList))
            {
                fromList = Url.Action("Index", "EventPeriods");
            }
            ViewBag.FromList = fromList;
            ViewData["EId"] = new SelectList(_context.Events, "Id", "Id", eventPeriod.EId);
            return View(eventPeriod);
        }

        // POST: EventPeriods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EId,StartTime,EndTime,Description")] EventPeriod eventPeriod)
        {
            if (id != eventPeriod.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventPeriod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventPeriodExists(eventPeriod.Id))
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
            ViewData["EId"] = new SelectList(_context.Events, "Id", "Id", eventPeriod.EId);
            return View(eventPeriod);
        }

        // GET: EventPeriods/Delete/5
        public async Task<IActionResult> Delete(int? id, string fromList)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventPeriod = await _context.EventPeriods
                .Include(e => e.EIdNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventPeriod == null)
            {
                return NotFound();
            }

            if (!Url.IsLocalUrl(fromList))
            {
                fromList = Url.Action("Index", "EventPeriods");
            }
            ViewBag.FromList = fromList;
            return View(eventPeriod);
        }

        // POST: EventPeriods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventPeriod = await _context.EventPeriods.FindAsync(id);
            if (eventPeriod != null)
            {
                _context.EventPeriods.Remove(eventPeriod);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventPeriodExists(int id)
        {
            return _context.EventPeriods.Any(e => e.Id == id);
        }
    }
}
