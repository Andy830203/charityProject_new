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
    public class SignUpsController : ControllerBase
    {
        private readonly charityContext _context;
        public List<EventPeriod> periods;

        public SignUpsController(charityContext context)
        {
            _context = context;
            periods = _context.EventPeriods.ToList();
        }

        // GET: api/SignUps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SignUpDTO>>> GetSignUps()
        {
            var SignUpDTO = await _context.SignUps
                .Include(i => i.ApplicantNavigation)
                .Include(i => i.Ep)
                .Select(item => new SignUpDTO
            {
                Id = item.Id,
                EpId = item.EpId,
                periodDesc = item.Ep.Description,
                Applicant = item.Applicant,
                ApplicantName = item.ApplicantNavigation.NickName
            }).ToListAsync();

            return SignUpDTO;
        }

        // GET: api/SignUps/event/5
        [HttpGet("event/{eid}")]
        public async Task<ActionResult<IEnumerable<SignUpDTO>>> GetSignUps(int eid)
        {
            var SignUpDTO = await _context.SignUps
                .Include(i => i.ApplicantNavigation)
                .Include(i => i.Ep)
                .Select(item => new SignUpDTO
                {
                    Id = item.Id,
                    EpId = item.EpId,
                    periodDesc = item.Ep.Description,
                    Applicant = item.Applicant,
                    ApplicantName = item.ApplicantNavigation.NickName
                }).ToListAsync();

            foreach (var item in SignUpDTO)
            {
                item.EId = periods.Where(ep => ep.Id == item.EpId).FirstOrDefault().EId;
            }
            var rtList = SignUpDTO.Where(item => item.EId == eid).ToList();

            return rtList;
        }

        // GET: api/SignUps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SignUpDTO>> GetSignUp(int id)
        {
            var signUp = await _context.SignUps.FindAsync(id);

            if (signUp == null)
            {
                return NotFound();
            }

            var signUpDTO = new SignUpDTO
            {
                Id = signUp.Id,
                EpId = signUp.EpId,
                periodDesc = signUp.Ep.Description,
                Applicant = signUp.Applicant,
                ApplicantName = signUp.ApplicantNavigation.NickName
            };

            return signUpDTO;
        }

        // PUT: api/SignUps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSignUp(int id, SignUpDTO signUpDTO)
        {
            if (id != signUpDTO.Id)
            {
                return BadRequest();
            }

            var signUp = new SignUp
            {
                Id = signUpDTO.Id,
                EpId = signUpDTO.EpId,
                Applicant = signUpDTO.Applicant,
            };

            _context.Entry(signUp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SignUpExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSignUp", new { id = signUp.Id }, signUp);
        }

        // POST: api/SignUps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SignUpDTO>> PostSignUp(SignUpDTO signUpDTO)
        {
            var signUp = new SignUp
            {
                Id = signUpDTO.Id,
                EpId = signUpDTO.EpId,
                Applicant = signUpDTO.Applicant,
            };

            _context.SignUps.Add(signUp);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSignUp", new { id = signUp.Id }, signUp);
        }

        // DELETE: api/SignUps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSignUp(int id)
        {
            var signUp = await _context.SignUps.FindAsync(id);
            if (signUp == null)
            {
                return NotFound();
            }

            _context.SignUps.Remove(signUp);
            await _context.SaveChangesAsync();

            return Ok("Delete success");
        }

        private bool SignUpExists(int id)
        {
            return _context.SignUps.Any(e => e.Id == id);
        }
    }
}
