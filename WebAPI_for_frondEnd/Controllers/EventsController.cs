using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_for_frondEnd.DTO;
using WebAPI_for_frondEnd.Models;

namespace WebAPI_for_frondEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly charityContext _context;
        public List<EventPeriodDTO> periods;

        public EventsController(charityContext context)
        {
            _context = context;

            periods =  _context.EventPeriods.Select(ep => new EventPeriodDTO
            {
                Id = ep.Id,
                EId = ep.EId,
                StartTime = ep.StartTime,
                EndTime = ep.EndTime,
                Description = ep.Description
            }).ToList();
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetEvents()
        {
            var EventsDTOs = await _context.Events
                .Include(item => item.Organizer)
                .Include(item => item.Category)
                .Select(item => new EventDTO
            {
                Id = item.Id,
                Name = item.Name,
                OrganizerId = item.OrganizerId,
                Organizer = item.Organizer.NickName,
                Description = item.Description,
                Fee = item.Fee,
                Capacity = item.Capacity,
                Priority = item.Priority,
                CategoryId = item.CategoryId,
                Category = item.Category.Name,
                Periods = new List<EventPeriodDTO>()
            }).ToListAsync();

            foreach (var item in EventsDTOs)
            {
                item.Periods = periods.Where(ep => ep.EId == item.Id).ToList();
            }
            //return await _context.Events.ToListAsync();
            return EventsDTOs;
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDTO>> GetEvent(int id)
        {
            var findEvent = await _context.Events.FindAsync(id);

            if (findEvent == null)
            {
                return NotFound();
            }

            var findEventDTO = new EventDTO
            {
                Id = findEvent.Id,
                Name = findEvent.Name,
                OrganizerId = findEvent.OrganizerId,
                Organizer = findEvent.Organizer.NickName,
                Description = findEvent.Description,
                Fee = findEvent.Fee,
                Capacity = findEvent.Capacity,
                Priority = findEvent.Priority,
                CategoryId = findEvent.CategoryId,
                Category = findEvent.Category.Name
            };

            findEventDTO.Periods = periods.Where(ep => ep.EId == findEventDTO.Id).ToList();

            return findEventDTO;
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, EventDTO eventDTO)
        {
            if (id != eventDTO.Id)
            {
                return BadRequest();
            }

            var modEvent = new Event
            {
                Id = eventDTO.Id,
                Name = eventDTO.Name,
                OrganizerId = eventDTO.OrganizerId,
                Description = eventDTO.Description,
                Fee = eventDTO.Fee,
                Capacity = eventDTO.Capacity,
                Priority = eventDTO.Priority,
                CategoryId = eventDTO.CategoryId
            };


            _context.Entry(modEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEvent", new { id = modEvent.Id }, modEvent);
        }

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EventDTO>> PostEvent(EventDTO newEventDTO)
        {
            var newEvent = new Event
            {
                Id = newEventDTO.Id, // default -1
                Name = newEventDTO.Name,
                OrganizerId = newEventDTO.OrganizerId,
                //Organizer = await _context.Members.Where(it => it.Id == newEventDTO.OrganizerId).FirstOrDefaultAsync(),
                Description = newEventDTO.Description,
                Fee = newEventDTO.Fee,
                Capacity = newEventDTO.Capacity,
                Priority = newEventDTO.Priority,
                CategoryId = newEventDTO.CategoryId,
                //Category = await _context.EventCategories.Where(it => it.Id == newEventDTO.CategoryId).FirstOrDefaultAsync(),
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = newEvent.Id }, newEvent);
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Events/Categories
        [HttpGet("Categories")]
        public async Task<ActionResult<IEnumerable<EventCategoryDTO>>> GetCategories()
        {
            var Categories = await _context.EventCategories
                .Select(it => new EventCategoryDTO
                {
                    categoryId = it.Id,
                    categoryName = it.Name
                }).ToListAsync();
            return Categories;
        }

        // GET: api/Events/Categories/5
        [HttpGet("Categories/{eid}")]
        public async Task<ActionResult<string>> GetCategoriesWithEid(int eid)
        {
            var Categories = await _context.EventCategories
                .Select(it => new EventCategoryDTO
                {
                    categoryId = it.Id,
                    categoryName = it.Name
                }).ToListAsync();

            var targetEvent = await _context.Events.Where(e => e.Id == eid).FirstOrDefaultAsync();

            var rtCateName = Categories.Where(c => c.categoryId == targetEvent?.CategoryId).FirstOrDefault()?.categoryName;
            return rtCateName ?? "NotFound";
        }

        // GET: api/Events/Locations/1
        [HttpGet("Locations/{eid}")]
        public ActionResult<IEnumerable<EventLocationDTO>> GetLocations(int eid)
        {
            return RedirectToAction("GetEventLocationByEvent", "EventLocations", new { eid });
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
