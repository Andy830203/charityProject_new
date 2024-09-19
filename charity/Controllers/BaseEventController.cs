using charity.Models;
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
        //public IActionResult Index()
        //{
        //    return View();
        //}

        //GET: /Events/EventLocations/5
        public virtual async Task<IActionResult> EventLocations(int id)
        {
            Event? e = await _context.Events.FindAsync(id);

            if (e == null)
            {
                return NotFound();
            }

            return PartialView("_EventLocationsPartial", e.EventLocations);
        }

        public virtual async Task<IActionResult> EventImgs(int id)
        {
            Event? e = await _context.Events.FindAsync(id);

            if (e == null)
            {
                return NotFound();
            }

            return PartialView("_EventImgsPartial", e.EventImgs);
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
