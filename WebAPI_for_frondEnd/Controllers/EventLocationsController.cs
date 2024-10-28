using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<IEnumerable<EventLocation>>> GetEventLocations()
        {
            return await _context.EventLocations.ToListAsync();
        }

        // GET: api/EventLocations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventLocation>> GetEventLocation(int id)
        {
            var eventLocation = await _context.EventLocations.FindAsync(id);

            if (eventLocation == null)
            {
                return NotFound();
            }

            return eventLocation;
        }

        // PUT: api/EventLocations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventLocation(int id, EventLocation eventLocation)
        {
            if (id != eventLocation.Id)
            {
                return BadRequest();
            }

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

            return NoContent();
        }

        // POST: api/EventLocations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EventLocation>> PostEventLocation(EventLocation eventLocation)
        {
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
