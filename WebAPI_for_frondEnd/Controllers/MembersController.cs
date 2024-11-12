using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CoreAPI2024;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
using WebAPI_for_frondEnd.DTO;
using WebAPI_for_frondEnd.Models;
using WebAPI_for_frondEnd.Service;


namespace WebAPI_for_frondEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly charityContext _context;
        private readonly UserService _userService;
        private readonly IMemoryCache _cache; // 注入 MemoryCache
        private readonly MailService _mailService;
        public MembersController(IMemoryCache cache, charityContext context, UserService userService, MailService mailService)
        {
            _context = context;
            _userService = userService;
            _cache = cache;
            _mailService = mailService;
        }

       // GET: api/Members
       //[HttpGet]
       // public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
       // {
       //     return await _context.Members.ToListAsync();
       // }

       // // GET: api/Members/5
       // [HttpGet("{id}")]
       // public async Task<ActionResult<Member>> GetMember(int id)
       // {
       //     var member = await _context.Members.FindAsync(id);

       //     if (member == null)
       //     {
       //         return NotFound();
       //     }

       //     return member;
       // }


        // GET: api/Members/5---所有會員資訊
        [HttpGet("member/{id}")]
        public async Task<MemberDTO> GetMember2(int id)
        {
            var memberdto = await _context.Members.FindAsync(id);

            if (memberdto == null)
            {
                return null;
            }
            string genderDisplay = memberdto.Gender.HasValue           //將bool轉完字串，顯示性別而非true/false
                            ? (memberdto.Gender.Value ? "男" : "女")
                            : "未指定";
            var mdto = new MemberDTO
            {

                memberid = memberdto.Id,
                memberaccount = memberdto.Account,
                memberpassword = memberdto.Password,
                membernickname = memberdto.NickName,
                membername = memberdto.RealName,
                membergender = genderDisplay,
                memberbirth = string.Format("{0:d}", memberdto.Birthday), //只顯示日期寫法
                memberemail = memberdto.Email,
                memberaddress = memberdto.Address,
                memberphone = memberdto.Phone,
                memberpoint = memberdto.Points,
                memberimg = memberdto.ImgName,
            };
            return mdto;
        }

        [HttpGet("api/member/status/{id}")] //帳戶狀況 
        public async Task<IActionResult> GetMemberStatus(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound("找不到會員。");
            }

            var statusDto = new StatusDTO
            {
                Status = member.Status == 1 ? "正常" : "停權",
            };

            return Ok(statusDto);
        }


        //GET: api/Members-------取得目前會員人數
        [HttpGet("count")]
        public async Task<IActionResult> GetMemberCount()
        {
            int memberCount = await _context.Members.CountAsync();
            var result = new MemberCountDto { Count = memberCount };
            return Ok(result);
        }

        // GET: api/Members/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            var member = await _context.Members.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return member;
        }


        // GET: api/Members/InFo/5---會員資訊(memberinfo)
        [HttpGet("InFo/{id}")]
        public async Task<MemberInFoDTO> GetMember1(int id)
        {
            var memberinfo = await _context.Members.FindAsync(id);


            if (memberinfo == null)
            {
                return null;
            }

            MemberInFoDTO minfo = new MemberInFoDTO
            {
                memberId = memberinfo.Id,
                membername = memberinfo.RealName,
                membernickname = memberinfo.NickName,
                memberemail = memberinfo.Email,
                memberpoint = memberinfo.Points,
                //memberbirth = memberinfo.Birthday.ToString(), "1994-2-28"
                memberbirth = string.Format("{0:d}", memberinfo.Birthday), //只顯示日期寫法
                memberexp = memberinfo.Exp,
                memberimg = memberinfo.ImgName
            };
            return minfo;
        }

        // GET: api/Members/GetMemberInFo/5---會員更改取得的資訊(GetMemberInFoDTO)
        [HttpGet("GetMemberInFo/{id}")]
        public async Task<GetMemberInFoDTO> GetMember3(int id)
        {
            var getinfo = await _context.Members.FindAsync(id);


            if (getinfo == null)
            {
                return null;
            }
            string genderDisplay = getinfo.Gender.HasValue           //將bool轉完字串，顯示性別而非true/false
                            ? (getinfo.Gender.Value ? "男" : "女")
                            : "未指定";

            GetMemberInFoDTO ginfo = new GetMemberInFoDTO
            {
                Id = getinfo.Id,
                ImgName = getinfo.ImgName,
                Nickname = getinfo.NickName,
                Name = getinfo.RealName,
                Birth = getinfo.Birthday?.ToString("yyyy/MM/dd"),
                //Birth = string.Format("{0:d}", getinfo.Birthday), //只顯示日期寫法
                Phone = getinfo.Phone,
                Address = getinfo.Address,
                Gender = getinfo.Gender,
                Email = getinfo.Email,
                GenderDisplay = genderDisplay, // 設定此屬性
            };
            return ginfo;
        }
        // 參加過的活動(評論用)
        [HttpGet("{userId}/activities")]
        public async Task<IActionResult> GetUserActivities(int userId)
        {
            // 確認會員是否存在
            var userExists = await _context.Members.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return NotFound(new { message = "會員不存在" });
            }

            // 使用 Join 查詢並映射到 UserActivityDto
            var activities = await (from s in _context.SignUps
                                    join d in _context.EventPeriods on s.EpId equals d.Id
                                    join e in _context.Events on d.EId equals e.Id
                                    where s.Applicant == userId
                                    select new GetMemberActivityDTO
                                    {
                                        SignUpId = s.Id,
                                        ActivityId = d.Id,
                                        EventId = e.Id,
                                        EventName = e.Name,
                                        EventDescription = e.Description
                                    }).ToListAsync();

            return Ok(activities);
        }


        // PUT: api/Members/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutMember(int id, Member member)
        //{
        //    if (id != member.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(member).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MemberExists(id))
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

        // PUT: api/Members/5----會員修改
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember1(int id, [FromForm] UpdateMember MemDTO)
        {
            if (id != MemDTO.Id)
            {
                return NotFound(new { message = "修改會員資料失敗!" });
            }

            Member Mem = await _context.Members.FindAsync(id);
            if (Mem == null)
            {
                return NotFound(new { message = "修改會員資料失敗!" });
            }

            // 更新會員基本資料
            Mem.NickName = MemDTO.Nickname;
            Mem.RealName = MemDTO.Name;
            Mem.Phone = MemDTO.Phone;
            Mem.Address = MemDTO.Address;
            Mem.Gender = MemDTO.Gender;
            if (!string.IsNullOrEmpty(MemDTO.Birth) && DateTime.TryParse(MemDTO.Birth, out var parsedDate))
            {
                Mem.Birthday = parsedDate;
            }
            else
            {
                Mem.Birthday = null;
            }

            // 更新圖片名稱
            if (!string.IsNullOrEmpty(MemDTO.ImgName))
            {
                Mem.ImgName = MemDTO.ImgName;
            }

            _context.Entry(Mem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
                {
                    return NotFound(new { message = "修改會員資料失敗!" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "修改會員資料成功!" });
        }

        [HttpPost("uploadProfilePicture")]
        public async Task<IActionResult> UploadProfilePicture([FromForm] IFormFile profilePicture)
        {
            if (profilePicture == null || profilePicture.Length == 0)
            {
                return BadRequest("未上傳圖片");
            }

            var fileName = $"{Guid.NewGuid()}_{profilePicture.FileName}";
            // 設定相對路徑
            var projectRoot = Directory.GetCurrentDirectory();
            var directoryPath = Path.Combine(projectRoot, "..", "charity", "wwwroot", "images", "members");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
            }

            return Ok(new { fileName = $"/images/members/{fileName}" });
        }

        // POST: api/Members
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Member>> PostMember(Member member)
        //{
        //    _context.Members.Add(member);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetMember", new { id = member.Id }, member);
        //}


        //// POST: api/Members-------註冊會員
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Register")]
        public async Task<RegisterDTO> PostMember(RegisterDTO MemDTO)
        {
            

            Member Mem = new Member
            {
                // Id = -1,
                Phone = string.IsNullOrEmpty(MemDTO.Phone) ? "nophone" : MemDTO.Phone,
                Address = string.IsNullOrEmpty(MemDTO.Address) ? "noaddress" : MemDTO.Address,
                //Phone = MemDTO.memberphone,
                //Address = MemDTO.memberaddress,
                Email = MemDTO.Email,
                Password = MemDTO.Password,
                RealName = MemDTO.Name,
                Gender = MemDTO.Gender ?? true,
                //Birthday = DateTime.Parse(MemDTO.memberbirth),
                Birthday = string.IsNullOrEmpty(MemDTO.Birth) ? DateTime.Now : DateTime.Parse(MemDTO.Birth),
            };
            //密碼加密加鹽
            var (hashedPassword, salt) = _userService.HashPassword(MemDTO.Password);

            // 保存加密後的密碼和鹽
            Mem.Password = hashedPassword;
            Mem.Account = salt;

            _context.Members.Add(Mem);
            await _context.SaveChangesAsync();

            return MemDTO;
        }




        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginDTO login)
        {
            var member = _context.Members.Where(m => m.Email.Equals(login.Email)).SingleOrDefault();
            if (member == null)
            {
                return NotFound(new { Message = "查無此帳號" });
            }
            var ID = member.Id;
            var Name = member.RealName;
            var hashedPassword = member.Password;
            var salt = member.Account; //Account是鹽

            // 檢查 MemoryCache 中是否存在臨時密碼
            if (_cache.TryGetValue(login.Email, out string cachedPassword))
            {
                // 驗證輸入的密碼是否匹配臨時密碼
                if (cachedPassword == login.Password)
                {
                    // 返回前端「強制更改密碼」頁面的 URL，而不是直接重導向 API
                    var redirectUrl = $"http://localhost:5173/ForceChangePassword";
                    return Ok(new { Message = "需要更改密碼", Status = "ForceChangePassword", RedirectUrl = redirectUrl, ID = ID, Name = Name });
                    // 臨時密碼驗證成功，重定向到強制更改密碼頁面
                    // RedirectToAction("ForceChangePassword", "Members", new { email = login.Email });
                }
                else
                {
                    return Unauthorized(new { Message = "臨時密碼錯誤" });
                }
            }
            var isPasswordValid = _userService.VerifyPassword(login.Password, hashedPassword, salt);

            if (!isPasswordValid)
            {
                return Unauthorized(new { Message = "密碼不正確" });
            }


            return Ok(new { Message = "登入成功",ID=ID, Name=Name});
        }

        // 強制更改密碼
        [HttpPost("ForceChangePassword")]
        public async Task<IActionResult> ForceChangePassword([FromBody] ChangePasswordDTO model)
        {
            if (model.NewPassword != model.ConfirmPassword)
            {
                return BadRequest("新密碼與確認密碼不符");
            }

            var member = await _context.Members.FirstOrDefaultAsync(m => m.Id == model.Id);
            if (member == null)
            {
                return Unauthorized("無法找到使用者");
            }

            // 更新密碼並將新密碼加密
            var(hashedPassword, salt) = _userService.HashPassword(model.NewPassword);
            member.Password = hashedPassword;
            member.Account = salt;
            await _context.SaveChangesAsync();

            // 清除 MemoryCache 中的臨時密碼
            _cache.Remove(member.Email);

            return Ok("密碼已成功更改");
        }

        // 忘記密碼請求
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            // 查找使用者
            var member = await _context.Members.FirstOrDefaultAsync(m => m.Email == model.Email);
            if (member == null)
            {
                return NotFound("該電子郵件地址未註冊");
            }

            // 生成隨機臨時密碼
            string temporaryPassword = GenerateRandomPassword();

            // 將臨時密碼暫存到 MemoryCache 中，有效時間設為 15 分鐘
            _cache.Set(member.Email, temporaryPassword, TimeSpan.FromMinutes(15));

            // 發送電子郵件
            string loginLink = "http://localhost:5173/"; // 登入頁面連結
            string emailContent = $"您的臨時密碼為：{temporaryPassword}。\n請點擊此連結以登入並更改您的密碼：{loginLink}";
            await _mailService.SendEmailAsync(member.Email,"", emailContent);

            return Ok($"臨時密碼已發送到您的信箱，您的臨時密碼為：{temporaryPassword}");
        }
        // 生成隨機密碼的方法
        private string GenerateRandomPassword(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        [HttpPost("mail")]
        public async Task<IActionResult> TRY([FromBody] string mail)
        {
            
            string emailContent = $"您的臨時密碼為：。\n請點擊此連結以登入並更改您的密碼：";
            await _mailService.SendEmailAsync(mail,"Charity官方--忘記密碼", emailContent);

            return Ok();
        }
        
    }


}
