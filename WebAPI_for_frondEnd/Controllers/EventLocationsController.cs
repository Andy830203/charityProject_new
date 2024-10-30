using System;
using System.Collections.Generic;
using System.Linq;
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
    public class EventLocationsController : ControllerBase
    {
        private readonly charityContext _context;

        public EventLocationsController(charityContext context)
        {
            _context = context;
        }

        // GET: api/EventLocations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventLocationDTO>>> GetEventLocations()
        {
            var ELocDTO = await _context.EventLocations
                .Include(e => e.LIdNavigation)
                .Include(e => e.EIdNavigation)
                .Select(e => new EventLocationDTO
                {
                    Id = e.Id,
                    LId = e.LId,
                    LocName = e.LIdNavigation.Name,
                    EId = e.EId,
                    belongedEvent = e.EIdNavigation.Name,
                    OrderInEvent = e.OrderInEvent,
                })
                .ToListAsync();

            return ELocDTO;
        }

        // GET: api/EventLocations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventLocationDTO>> GetEventLocation(int id)
        {
            var eventLocation = await _context.EventLocations.FindAsync(id);

            if (eventLocation == null)
            {
                return NotFound();
            }

            var ELocDTO = new EventLocationDTO
            {
                Id = eventLocation.Id,
                LId = eventLocation.LId,
                LocName = eventLocation.LIdNavigation.Name,
                EId = eventLocation.EId,
                belongedEvent = eventLocation.EIdNavigation.Name,
                OrderInEvent = eventLocation.OrderInEvent,
            };

            return ELocDTO;
        }

        // GET: api/EventLocations/Event/5
        [HttpGet("Event/{eid}")]
        public async Task<ActionResult<IEnumerable<EventLocationDTO>>> GetEventLocationByEvent(int eid)
        {
            var ELocs = await _context.EventLocations
                .Where(x => x.EId == eid)
                .Include(e => e.LIdNavigation)
                .Include(e => e.EIdNavigation)
                .Select(e => new EventLocationDTO
                {
                    Id = e.Id,
                    LId = e.LId,
                    LocName = e.LIdNavigation.Name,
                    EId = e.EId,
                    belongedEvent = e.EIdNavigation.Name,
                    OrderInEvent = e.OrderInEvent,
                }).ToListAsync();

            return ELocs;
        }

        // PUT: api/EventLocations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventLocation(int id, EventLocationDTO eventLocDTO)
        {
            if (id != eventLocDTO.Id)
            {
                return BadRequest();
            }

            var eventLocation = new EventLocation
            {
                Id = eventLocDTO.Id,
                LId = eventLocDTO.LId,
                EId = eventLocDTO.EId,
                OrderInEvent = eventLocDTO.OrderInEvent
            };

            _context.Entry(eventLocation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventLocationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEventLocation", new { id = eventLocation.Id }, eventLocation);
        }

        // POST: api/EventLocations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EventLocationDTO>> PostEventLocation(EventLocationDTO eventLocDTO)
        {
            var eventLocation = new EventLocation
            {
                Id = eventLocDTO.Id,
                LId = eventLocDTO.LId,
                EId = eventLocDTO.EId,
                OrderInEvent = eventLocDTO.OrderInEvent
            };
            _context.EventLocations.Add(eventLocation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEventLocation", new { id = eventLocation.Id }, eventLocation);
        }

        // DELETE: api/EventLocations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventLocation(int id)
        {
            var eventLocation = await _context.EventLocations.FindAsync(id);
            if (eventLocation == null)
            {
                return NotFound();
            }

            _context.EventLocations.Remove(eventLocation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventLocationExists(int id)
        {
            return _context.EventLocations.Any(e => e.Id == id);
        }
    }
}
