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
    public class CollectionsController : ControllerBase
    {
        private readonly charityContext _context;

        public CollectionsController(charityContext context)
        {
            _context = context;
        }

        // 獲取用戶的收藏活動名稱
        [HttpGet("{memberId}")]
        public async Task<IActionResult> GetUserFavorites(int memberId)
        {
            try
            {
                var favorites = await (from collection in _context.Collection
                                       join eventItem in _context.Events
                                       on collection.eventId equals eventItem.Id
                                       where collection.MemberId == memberId
                                       select new
                                       {
                                           EventName = eventItem.Name,
                                           CollectionId = collection.Id,
                                           EventId = collection.eventId
                                       }).ToListAsync();

                return Ok(favorites);
            }
            catch (Exception ex)
            {
                // Log the exception as needed, or inspect the 'ex' for more details.
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // GET: api/Collections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Collection>>> GetCollection()
        {
            return await _context.Collection.ToListAsync();
        }

        //GET: api/Collections/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Collection>> GetCollection(int id)
        //{
        //    var collection = await _context.Collection.FindAsync(id);

        //    if (collection == null)
        //    {
        //        return NotFound();
        //    }

        //    return collection;
        //}

        // PUT: api/Collections/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCollection(int id, Collection collection)
        //{
        //    if (id != collection.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(collection).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CollectionExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Collections
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Collection>> PostCollection(Collection collection)
        //{
        //    _context.Collection.Add(collection);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetCollection", new { id = collection.Id }, collection);
        //}

        // 添加活動到收藏的API
        [HttpPost("AddFavorite")]
        public async Task<IActionResult> AddToFavorites([FromBody] AddFavoriteDTO dto)
        {
            // 檢查是否已經收藏此活動
            var existingFavorite = await _context.Collection
                                                 .FirstOrDefaultAsync(f => f.MemberId == dto.MemberId && f.eventId == dto.EventId);
            if (existingFavorite != null)
            {
                return BadRequest("活動已在收藏中。");
            }

            var favorite = new Collection
            {
                MemberId = dto.MemberId,
                eventId = dto.EventId,
                attendance = false // 新收藏的活動，初始狀態為未參加
            };

            _context.Collection.Add(favorite);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserFavorites), new { memberId = dto.MemberId }, dto);
        }

        // 更新參加狀態的API
        [HttpPut("UpdateAttendance/{id}")]
        public async Task<IActionResult> UpdateAttendance(int id, [FromBody] UpdateAttendanceDTO dto)
        {
            // 確認該收藏活動是否存在
            var favorite = await _context.Collection.FindAsync(id);
            if (favorite == null)
            {
                return NotFound();
            }

            favorite.attendance = dto.Attendance;
            await _context.SaveChangesAsync();

            return Ok(new FavoriteListDTO
            {
                Id = favorite.Id,
                eventId = favorite.eventId,
                attendance = favorite.attendance
            });
        }

        // 從收藏中移除活動的API
        [HttpDelete("{memberId}/{eventId}")]
        public async Task<IActionResult> RemoveFromFavorites(int memberId, int eventId)
        {
            var favorite = await _context.Collection
                                         .FirstOrDefaultAsync(f => f.MemberId == memberId && f.eventId == eventId);
            if (favorite == null)
            {
                return NotFound();
            }

            _context.Collection.Remove(favorite);
            await _context.SaveChangesAsync();

            return Ok("Deleted successfully");
        }

        // DELETE: api/Collections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollection(int id)
        {
            var collection = await _context.Collection.FindAsync(id);
            if (collection == null)
            {
                return NotFound();
            }

            _context.Collection.Remove(collection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CollectionExists(int id)
        {
            return _context.Collection.Any(e => e.Id == id);
        }
    }
}
