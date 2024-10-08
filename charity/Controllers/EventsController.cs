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
    public class EventsController : BaseEventController
    {
        //private readonly CharityContext _context;

        public EventsController(CharityContext context) : base(context) 
        {
            //_context = context;
        }

        // GET: Events
        //public async Task<IActionResult> Index()
        //{
        //    var charityContext = _context.Events.Include(e => e.Category).Include(e => e.Organizer);
        //    return View(await charityContext.ToListAsync());
        //}

        public IActionResult Index()
        {
            var events = _context.Events.Include(e => e.Category).Include(e => e.Organizer)
                .ToList();

            var eventsWithPeriods = events.SelectMany(e => e.EventPeriods, (e, period) => new { EventSelf = e, EventPeriod = period, Name = e.Name });

            var appliants = _context.SignUps.ToList();

            //?? 活動地點綁'活動時段', 優先級或活動本身
            var charityContext = eventsWithPeriods
                .Select(ewps => new EventsViewModel {
                    Id = ewps.EventSelf.Id,
                    Priority = ewps.EventSelf.Priority,
                    Category = ewps.EventSelf.Category.Name,
                    Name = ewps.EventSelf.Name,
                    Count = appliants.Where(appliant => appliant.EpId == ewps.EventPeriod.Id).Count(),
                    Location = ewps.EventSelf.EventLocations.FirstOrDefault(),
                    PeriodDesc = ewps.EventPeriod.Description,
                    Period = $"{ewps.EventPeriod.StartTime.ToString()} ~ {ewps.EventPeriod.EndTime.ToString()}",
                    Fee = ewps.EventSelf.Fee,
                    Capacity = ewps.EventSelf.Capacity,
                    ImageName = ewps.EventSelf.EventImgs.FirstOrDefault().ImgName,
                });
            return View(charityContext);
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.EventCategories, "Id", "Id");
            ViewData["OrganizerId"] = new SelectList(_context.Members, "Id", "Id");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,OrganizerId,Fee,Capacity,Description,Priority,CategoryId")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.EventCategories, "Id", "Id", @event.CategoryId);
            ViewData["OrganizerId"] = new SelectList(_context.Members, "Id", "Id", @event.OrganizerId);
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.EventCategories, "Id", "Id", @event.CategoryId);
            ViewData["OrganizerId"] = new SelectList(_context.Members, "Id", "Id", @event.OrganizerId);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,OrganizerId,Fee,Capacity,Description,Priority,CategoryId")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.EventCategories, "Id", "Id", @event.CategoryId);
            ViewData["OrganizerId"] = new SelectList(_context.Members, "Id", "Id", @event.OrganizerId);
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
