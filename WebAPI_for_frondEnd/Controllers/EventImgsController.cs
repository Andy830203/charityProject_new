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
    public class EventImgsController : ControllerBase
    {
        private readonly charityContext _context;

        public EventImgsController(charityContext context)
        {
            _context = context;
        }

        // GET: api/EventImgs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventImgDTO>>> GetEventImgs()
        {
            var EImgDTOs = await _context.EventImgs.Select(em => new EventImgDTO
            {
                Id = em.Id,
                EId = em.Id,
                ImgName = em.ImgName,
            }).ToListAsync();

            return EImgDTOs;
        }

        // GET: api/EventImgs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventImgDTO>> GetEventImg(int id)
        {
            var eventImg = await _context.EventImgs.FindAsync(id);

            if (eventImg == null)
            {
                return NotFound();
            }

            var EImg = new EventImgDTO { 
                Id = eventImg.Id,
                EId = eventImg.EId,
                ImgName = eventImg.ImgName,
            };

            return EImg;
        }

        // GET: api/EventImgs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventImgDTO>> GetEventImg(int id)
        {
            var eventImg = await _context.EventImgs.FindAsync(id);

            if (eventImg == null)
            {
                return NotFound();
            }

            var EImg = new EventImgDTO
            {
                Id = eventImg.Id,
                EId = eventImg.EId,
                ImgName = eventImg.ImgName,
            };

            return EImg;
        }

        // PUT: api/EventImgs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventImg(int id, EventImgDTO eventImgDTO)
        {
            if (id != eventImgDTO.Id)
            {
                return BadRequest();
            }

            var eventImg = new EventImg
            {
                Id = eventImgDTO.Id,
                EId = eventImgDTO.EId,
                ImgName = eventImgDTO.ImgName,
            };

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
        public async Task<ActionResult<EventImgDTO>> PostEventImg([FromForm] EventImgDTO eventImgDTO, IFormFile updatePic)
        {
            string? fileName = null;
            if (updatePic == null || updatePic.Length == 0)
            {
                return BadRequest("未上傳圖片");
            }
            
            if (updatePic.FileName == eventImgDTO.ImgName)
            {
                fileName = $"{Guid.NewGuid()}_{updatePic.FileName}";
            }

            // 設定相對路徑
            var projectRoot = Directory.GetCurrentDirectory();
            var directoryPath = Path.Combine(projectRoot, "..", "charity", "wwwroot", "images", "Events");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await updatePic.CopyToAsync(stream);
            }

            var eventImg = new EventImg
            {
                EId = eventImgDTO.EId,
                ImgName = fileName??"",
            };
            _context.EventImgs.Add(eventImg);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetEventImg", new { id = eventImg.Id }, eventImg);
            return Ok(new { fileName = $"/images/Events/{fileName}" });
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
