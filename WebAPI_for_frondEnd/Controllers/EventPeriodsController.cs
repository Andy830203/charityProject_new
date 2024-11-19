using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_for_frondEnd.Models;
using WebAPI_for_frondEnd.DTO;

namespace WebAPI_for_frondEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventPeriodsController : ControllerBase
    {
        private readonly charityContext _context;

        public EventPeriodsController(charityContext context)
        {
            _context = context;
        }

        // GET: api/EventPeriods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventPeriodDTO>>> GetEventPeriods()
        {
            var EPDTOs = await _context.EventPeriods.Select(ep => new EventPeriodDTO
            {
                Id = ep.Id,
                EId = ep.EId,
                StartTime = ep.StartTime,
                EndTime = ep.EndTime,
                Description = ep.Description,
            }).ToListAsync();
            return EPDTOs;
        }

        // GET: api/EventPeriods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventPeriodDTO>> GetEventPeriod(int id)
        {
            var eventPeriod = await _context.EventPeriods.FindAsync(id);

            if (eventPeriod == null)
            {
                return NotFound();
            }

            var eventPeriodDTO = new EventPeriodDTO
            {
                Id = eventPeriod.Id,
                EId = eventPeriod.EId,
                StartTime = eventPeriod.StartTime,
                EndTime = eventPeriod.EndTime,
                Description = eventPeriod.Description,
            };

            return eventPeriodDTO;
        }

        // PUT: api/EventPeriods/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventPeriod(int id, EventPeriodDTO eventPeriodDTO)
        {
            if (id != eventPeriodDTO.Id)
            {
                return BadRequest();
            }

            var eventPeriod = new EventPeriod
            {
                Id = eventPeriodDTO.Id,
                EId = eventPeriodDTO.EId,
                StartTime = eventPeriodDTO.StartTime,
                EndTime = eventPeriodDTO.EndTime,
                Description = eventPeriodDTO.Description,
            };

            _context.Entry(eventPeriod).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventPeriodExists(id))
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

        // POST: api/EventPeriods
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EventPeriod>> PostEventPeriod(EventPeriodDTO eventPeriodDTO)
        {
            var eventPeriod = new EventPeriod
            {
                Id = eventPeriodDTO.Id,
                EId = eventPeriodDTO.EId,
                StartTime = eventPeriodDTO.StartTime,
                EndTime = eventPeriodDTO.EndTime,
                Description = eventPeriodDTO.Description,
            };

            _context.EventPeriods.Add(eventPeriod);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEventPeriod", new { id = eventPeriod.Id }, eventPeriod);
        }

        // DELETE: api/EventPeriods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventPeriod(int id)
        {
            var eventPeriod = await _context.EventPeriods.FindAsync(id);
            if (eventPeriod == null)
            {
                return NotFound();
            }

            _context.EventPeriods.Remove(eventPeriod);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventPeriodExists(int id)
        {
            return _context.EventPeriods.Any(e => e.Id == id);
        }
    }
}
