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
    public class EventLocationsController : Controller
    {
        private readonly CharityContext _context;

        public EventLocationsController(CharityContext context)
        {
            _context = context;
        }

        // GET: EventLocations
        public async Task<IActionResult> Index()
        {
            var charityContext = _context.EventLocations.Include(e => e.EIdNavigation).Include(e => e.LIdNavigation);
            return View(await charityContext.ToListAsync());
        }

        // GET: EventLocations/Details/5
        public async Task<IActionResult> Details(int? id, string? fromList)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventLocation = await _context.EventLocations
                .Include(e => e.EIdNavigation)
                .Include(e => e.LIdNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventLocation == null)
            {
                return NotFound();
            }

            if (!Url.IsLocalUrl(fromList))
            {
                fromList = Url.Action("Index", "EventLocations");
            }
            ViewBag.FromList = fromList;
            return View(eventLocation);
        }

        // GET: EventLocations/Create
        public IActionResult Create(string? fromList)
        {
            if (!Url.IsLocalUrl(fromList))
            {
                fromList = Url.Action("Index", "EventLocations");
            }
            ViewData["EId"] = new SelectList(_context.Events, "Id", "Id");
            ViewData["LId"] = new SelectList(_context.Locations, "Id", "Id");
            ViewBag.FromList = fromList;
            return View();
        }

        // POST: EventLocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string? fromList,[Bind("Id,EId,LId,OrderInEvent")] EventLocation eventLocation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventLocation);
                await _context.SaveChangesAsync();
                if (!string.IsNullOrEmpty(fromList))
                {
                    return RedirectToAction("Index", fromList);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EId"] = new SelectList(_context.Events, "Id", "Id", eventLocation.EId);
            ViewData["LId"] = new SelectList(_context.Locations, "Id", "Id", eventLocation.LId);
            if (!string.IsNullOrEmpty(fromList))
            {
                return View(fromList, eventLocation);
            }
            return View(eventLocation);
        }

        // GET: EventLocations/Edit/5
        public async Task<IActionResult> Edit(int? id, string? fromList)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventLocation = await _context.EventLocations.FindAsync(id);
            if (eventLocation == null)
            {
                return NotFound();
            }

            if (!Url.IsLocalUrl(fromList))
            {
                fromList = Url.Action("Index", "EventLocations");
            }
            ViewBag.FromList = fromList;
            ViewData["EId"] = new SelectList(_context.Events, "Id", "Id", eventLocation.EId);
            ViewData["LId"] = new SelectList(_context.Locations, "Id", "Id", eventLocation.LId);
            return View(eventLocation);
        }

        // POST: EventLocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string fromList, [Bind("Id,EId,LId,OrderInEvent")] EventLocation eventLocation)
        {
            if (id != eventLocation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventLocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventLocationExists(eventLocation.Id))
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
            ViewData["EId"] = new SelectList(_context.Events, "Id", "Id", eventLocation.EId);
            ViewData["LId"] = new SelectList(_context.Locations, "Id", "Id", eventLocation.LId);
            if (!string.IsNullOrEmpty(fromList))
            {
                return View(fromList, eventLocation);
            }
            return View(eventLocation);
        }

        // GET: EventLocations/Delete/5
        public async Task<IActionResult> Delete(int? id, string? fromList)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventLocation = await _context.EventLocations
                .Include(e => e.EIdNavigation)
                .Include(e => e.LIdNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventLocation == null)
            {
                return NotFound();
            }

            if (!Url.IsLocalUrl(fromList))
            {
                fromList = Url.Action("Index", "EventLocations");
            }
            ViewBag.FromList = fromList;
            return View(eventLocation);
        }

        // POST: EventLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string fromList)
        {
            var eventLocation = await _context.EventLocations.FindAsync(id);
            if (eventLocation != null)
            {
                _context.EventLocations.Remove(eventLocation);
            }

            await _context.SaveChangesAsync();
            if (!string.IsNullOrEmpty(fromList))
            {
                return RedirectToAction("Index", fromList);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EventLocationExists(int id)
        {
            return _context.EventLocations.Any(e => e.Id == id);
        }
    }
}
