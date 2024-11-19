using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_for_frondEnd.DTO;
using WebAPI_for_frondEnd.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            // 使用 join 連接 SignUps 和 Periods 表
            var SignUpDTO = await (
                from signUp in _context.SignUps
                join period in _context.EventPeriods on signUp.EpId equals period.Id
                select new SignUpDTO
                {
                    Id = signUp.Id,
                    EId = period.EId,
                    EpId = signUp.EpId,
                    periodDesc = signUp.Ep.Description,
                    Applicant = signUp.Applicant,
                    ApplicantName = signUp.ApplicantNavigation.NickName
                }
            ).ToListAsync();

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

            var eid = periods.Where(it => it.Id == signUp.EpId).FirstOrDefault()?.EId;

            var signUpDTO = new SignUpDTO
            {
                Id = signUp.Id,
                EId = eid,
                EpId = signUp.EpId,
                periodDesc = signUp.Ep.Description,
                Applicant = signUp.Applicant,
                ApplicantName = signUp.ApplicantNavigation.NickName
            };

            return signUpDTO;
        }

        // GET: api/SignUps/isRepeat/5/1
        [HttpGet("isRepeat/{epid}/{memberId}")]
        public async Task<ActionResult<string>> GetSignUpIsRepeat(int epid, int memberId)
        {
            var SignUpDTOs = await _context.SignUps
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

            var epidMembers = SignUpDTOs
                .Where(item => item.EpId == epid)
                .Where(item => item.Applicant == memberId)
                .ToList();

            int counts = epidMembers.Count;

            return counts switch
            {
                0 => "NotExist",
                1 => "Exist",
                _ => "Repeated",
            };
        }

        // 增加一事件有多少人報名
        // GET: api/SignUps/HowMany/5 (eid)
        [HttpGet("HowMany/{eid}")]
        public async Task<ActionResult<int>> GetSignUpIsRepeat(int eid)
        {
            var SignUpDTOs = await _context.SignUps
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

            var EPDTOs = await _context.EventPeriods.Select(ep => new EventPeriodDTO
            {
                Id = ep.Id,
                EId = ep.EId,
                StartTime = ep.StartTime,
                EndTime = ep.EndTime,
                Description = ep.Description,
            }).ToListAsync();
            
            var SignUpWithEPeriods = SignUpDTOs.Join(EPDTOs,
                signUps => signUps.EpId,  // Key from SignUpDTOs
                EPs => EPs.Id,           // Key from EPDTOs
                (signUps, EPs) => new    // Result selector for the joined data
                {
                    EPs.EId,       // Extract EId from EPDTOs
                    signUps.Id,
                    signUps.EpId,
                    signUps.periodDesc,
                    signUps.Applicant,
                    signUps.ApplicantName
                }).ToList();

            var theList = SignUpWithEPeriods.Where(sep => sep.EId == eid).ToList();

            return theList.Count;
        }

        // GET: api/SignUps/member/5
        [HttpGet("member/{userId}")]
        public async Task<ActionResult<SignUpDTO>> GetSignUpByUser(int userId) {
            // 查詢報名者的報名記錄，並通過 EventPeriod 找到對應的活動
            var events = await _context.SignUps
                .Where(signUp => signUp.Applicant == userId) // 過濾 Applicant
                .Include(signUp => signUp.Ep) // 加載 EventPeriod
                    .ThenInclude(ep => ep.EIdNavigation) // 加載 Event
                .Select(signUp => new {
                    EventId = signUp.Ep.EId, // 活動 ID
                    EventPeriodId = signUp.Ep.Id, // 活動時段ID
                    EventName = signUp.Ep.EIdNavigation.Name, // 活動名稱
                    PeriodDescription = signUp.Ep.Description, // 時段描述
                    PeriodStart = signUp.Ep.StartTime, // 時段開始時間
                    PeriodEnd = signUp.Ep.EndTime // 時段結束時間
                })
                .ToListAsync();

            //if (events == null || events.Count == 0) {
            //    return NotFound(new { message = "此報名者尚未報名任何活動。" });
            //}

            return Ok(events);
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
                EpId = signUpDTO.EpId,
                Applicant = signUpDTO.Applicant,
            };

            _context.SignUps.Add(signUp);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSignUp", new { id = signUp.Id }, signUpDTO);
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

        [HttpDelete("DeleteByApplicantAndPeriod")]
        public async Task<IActionResult> DeleteByApplicantAndPeriod([FromQuery] int applicantId, [FromQuery] int periodId) {
            // 查詢符合條件的 SignUp 記錄
            var signUpsToDelete = await _context.SignUps
                .Where(s => s.Applicant == applicantId && s.EpId == periodId)
                .ToListAsync();

            if (!signUpsToDelete.Any()) {
                return NotFound(new { message = "找不到符合條件的報名記錄。" });
            }

            // 刪除記錄
            _context.SignUps.RemoveRange(signUpsToDelete);
            await _context.SaveChangesAsync();

            return Ok(new {
                message = "報名記錄已成功刪除。",
                deletedCount = signUpsToDelete.Count
            });
        }
        private bool SignUpExists(int id)
        {
            return _context.SignUps.Any(e => e.Id == id);
        }
    }
}
