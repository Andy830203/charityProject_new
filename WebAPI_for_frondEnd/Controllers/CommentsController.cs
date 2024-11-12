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
    public class CommentsController : ControllerBase
    {
        private readonly charityContext _context;

        public CommentsController(charityContext context)
        {
            _context = context;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            return await _context.Comments.ToListAsync();
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        // PUT: api/Comments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Comment>> PostComment(Comment comment)
        //{
        //    _context.Comments.Add(comment);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
        //}

        //評論與回傳
        [HttpPost("postComment")]
        public async Task<IActionResult> PostComment([FromBody] CommentDTO mmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 檢查是否已經存在該用戶對此活動的評論
            bool hasCommented = await _context.Comments
                .AnyAsync(c => c.MId == mmentDto.m_id && c.EId == mmentDto.e_id);

            if (hasCommented)
            {
                return BadRequest("");
            }


            var comment = new Comment
            {
                EId = mmentDto.e_id,
                MId = mmentDto.m_id,
                score = mmentDto.score,
                Content = mmentDto.content
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // 創建回傳的 CommentResponseDto
            var responseDto = new CommentResponseDTO
            {
                id = comment.Id,
                e_id = comment.EId,
                m_id = comment.MId,
                score = comment.score,
                content = comment.Content,
            };

            return Ok(responseDto);
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
