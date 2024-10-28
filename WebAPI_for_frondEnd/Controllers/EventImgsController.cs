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
    public class EventImgsController : ControllerBase
    {
        private readonly charityContext _context;

        public EventImgsController(charityContext context)
        {
            _context = context;
        }

        // GET: api/EventImgs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventImg>>> GetEventImgs()
        {
            return await _context.EventImgs.ToListAsync();
        }

        // GET: api/EventImgs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventImg>> GetEventImg(int id)
        {
            var eventImg = await _context.EventImgs.FindAsync(id);

            if (eventImg == null)
            {
                return NotFound();
            }

            return eventImg;
        }

        // PUT: api/EventImgs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventImg(int id, EventImg eventImg)
        {
            if (id != eventImg.Id)
            {
                return BadRequest();
            }

            _context.Entry(eventImg).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventImgExists(id))
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

        // POST: api/EventImgs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EventImg>> PostEventImg(EventImg eventImg)
        {
            _context.EventImgs.Add(eventImg);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEventImg", new { id = eventImg.Id }, eventImg);
        }

        // DELETE: api/EventImgs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventImg(int id)
        {
            var eventImg = await _context.EventImgs.FindAsync(id);
            if (eventImg == null)
            {
                return NotFound();
            }

            _context.EventImgs.Remove(eventImg);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventImgExists(int id)
        {
            return _context.EventImgs.Any(e => e.Id == id);
        }
    }
}
