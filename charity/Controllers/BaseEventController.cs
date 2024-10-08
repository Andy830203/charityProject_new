using charity.Models;
using charity.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace charity.Controllers
{
    public abstract class BaseEventController : Controller
    {
        protected readonly CharityContext _context;

        public BaseEventController(CharityContext context)
        {
            _context = context;
        }

        //GET: /Events/EventLocations/5
        public virtual async Task<IActionResult> EventLocations(int id)
        {
            Event? e = await _context.Events.FindAsync(id);

            if (e == null)
            {
                return NotFound();
            }

            var locations = _context.Locations;

            var EventLocVMs = e.EventLocations.Select(loc => new EventLocViewModel
            {
                Id = loc.Id,
                EventName = e.Name,
                EventCapacity = e.Capacity,
                LocationName = locations.Where(l => l.Id == loc.LId).Select(l => l.Name).Single(),
                LocationDesc = locations.Where(l => l.Id == loc.LId).Select(l => l.Description).Single(),
                LocCapacity = locations.Where(l => l.Id == loc.LId).Select(l => l.Capacity).Single(),
                Order = loc.OrderInEvent
            });

            return PartialView("_EventLocationsPartial", EventLocVMs);
        }

        public virtual async Task<IActionResult> EventImgs(int id)
        {
            Event? e = await _context.Events.FindAsync(id);

            if (e == null)
            {
                return NotFound();
            }

            var EImgVMs = e.EventImgs.Select(ei => new EventImgViewModel {
                Id = ei.Id,
                EId = ei.EId,
                ImgName = ei.ImgName,
                EventName = e.Name
            });

            return PartialView("_EventImgsPartial", EImgVMs);
        }

        public virtual async Task<IActionResult> EventPeriods(int id)
        {
            Event? e = await _context.Events.FindAsync(id);

            if (e == null)
            {
                return NotFound();
            }

            return PartialView("_EventPeriodsPartial", e.EventPeriods);
        }
    }
}
