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
    public class LocationsController : ControllerBase
    {
        private readonly charityContext _context;

        public LocationsController(charityContext context)
        {
            _context = context;
        }

        // GET: api/Locations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location_mapinfo_DTO>>> GetLocations()
        {
            var locations = await _context.Locations
                .Select(e => new Location_mapinfo_DTO {
                    Id = e.Id,
                    Name = e.Name,
                    Longitude = e.Longitude,
                    Latitude = e.Latitude,
                    Description = e.Description,
                    Address = e.Address,
                    PlusCode = e.PlusCode,
                    Capacity = e.Capacity
                })
                .ToListAsync();
            return locations;
        }
        // GET: api/Locations
        [HttpGet("Location_maploc")]
        public async Task<ActionResult<IEnumerable<Location_maploc_DTO>>> GetLocations_lngat()
        {
            var locations_gm = await _context.Locations
                .Select(e => new Location_maploc_DTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    Longitude = e.Longitude,
                    Latitude = e.Latitude,
                    Address = e.Address,
                })
                .ToListAsync();
            return locations_gm;
        }

        // GET: api/Locations/Location_maploc_Info
        //[HttpGet("Location_maploc_Info")]
        //public async Task<ActionResult<IEnumerable<EventLocationDTO_formap_onlyDTO>>> GetLocations_maploc_Info()
        //{
        //    var locations_gm = await _context.Locations
        //        .Include(l => l.EventLocations) // 加入 EventLocation 關聯
        //            .ThenInclude(el => el.Event) // 加入 Event 關聯
        //        .Select(e => new EventLocationDTO_formap_onlyDTO
        //        {
        //            Id = e.Id,
        //            LId = e.EventLocations.FirstOrDefault().LId, // 假設一個 Location 對應到多個 EventLocation
        //            LocName = e.LocName,
        //            EId = e.EventLocations.FirstOrDefault().EId, // 取得第一個對應的 EventLocation
        //            Address = e.Address,
        //            EventName = e.EventLocations.FirstOrDefault().Event?.Name // 取得第一個 Event 名稱
        //        })
        //        .ToListAsync();

        //    return locations_gm;
        //}


        // GET: api/Locations/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Location>> GetLocation(int id)
        //{
        //    var location = await _context.Locations.FindAsync(id);

        //    if (location == null)
        //    {
        //        return NotFound();
        //    }

        //    return location;
        //}
        [HttpGet("{id}")]
        public async Task<ActionResult<LocationDto>> GetLocation(int id)
        {
            var location = await _context.Locations
                .Include(l => l.EventLocations)
                .Include(l => l.LocationImgs)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (location == null)
            {
                return NotFound();
            }

            // 將 Location 轉換成 LocationDto
            var locationDto = new LocationDto
            {
                Id = location.Id,
                Name = location.Name,
                Longitude = location.Longitude,
                Latitude = location.Latitude,
                Address = location.Address,
                Capacity = location.Capacity,
            };
            return locationDto;
        }

        // PUT: api/Locations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(int id, Location location)
        {
            if (id != location.Id)
            {
                return BadRequest();
            }

            _context.Entry(location).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
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

        // POST: api/Locations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Location>> PostLocation(Location location)
        {
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocation", new { id = location.Id }, location);
        }

        // DELETE: api/Locations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.Id == id);
        }
    }
}
