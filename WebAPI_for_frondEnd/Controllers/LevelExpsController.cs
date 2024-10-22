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
    public class LevelExpsController : ControllerBase
    {
        private readonly charityContext _context;

        public LevelExpsController(charityContext context)
        {
            _context = context;
        }

        // GET: api/LevelExps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LevelExp>>> GetLevelExps()
        {
            return await _context.LevelExps.ToListAsync();
        }

        // GET: api/LevelExps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LevelExp>> GetLevelExp(int id)
        {
            var levelExp = await _context.LevelExps.FindAsync(id);

            if (levelExp == null)
            {
                return NotFound();
            }

            return levelExp;
        }

        // PUT: api/LevelExps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLevelExp(int id, LevelExp levelExp)
        {
            if (id != levelExp.Level)
            {
                return BadRequest();
            }

            _context.Entry(levelExp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LevelExpExists(id))
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

        // POST: api/LevelExps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LevelExp>> PostLevelExp(LevelExp levelExp)
        {
            _context.LevelExps.Add(levelExp);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LevelExpExists(levelExp.Level))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLevelExp", new { id = levelExp.Level }, levelExp);
        }

        // DELETE: api/LevelExps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLevelExp(int id)
        {
            var levelExp = await _context.LevelExps.FindAsync(id);
            if (levelExp == null)
            {
                return NotFound();
            }

            _context.LevelExps.Remove(levelExp);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LevelExpExists(int id)
        {
            return _context.LevelExps.Any(e => e.Level == id);
        }
    }
}
