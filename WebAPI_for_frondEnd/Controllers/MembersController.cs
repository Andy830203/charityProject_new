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
using WebAPI_for_frondEnd.DTO;
using WebAPI_for_frondEnd.Models;

namespace WebAPI_for_frondEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly charityContext _context;
        private readonly UserService _userService;

        public MembersController(charityContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: api/Members
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        //{
        //    return await _context.Members.ToListAsync();
        //}

        //// GET: api/Members/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Member>> GetMember(int id)
        //{
        //    var member = await _context.Members.FindAsync(id);

        //    if (member == null)
        //    {
        //        return NotFound();
        //    }

        //    return member;
        //}


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




        // GET: api/Members/InFo/5---會員資訊(memberinfo)
        [HttpGet("InFo/{id}")]
        public async Task<MemberInFoDTO> GetMember1(int id)
        {
            var memberinfo = await _context.Members.FindAsync(id);
            var expinfo = await _context.LevelExps.FindAsync(id);


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
                memberlevel = expinfo.Level,
            };
            return minfo;
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
        public async Task<string> PutMember1(int id, MemberDTO MemDTO)
        {
            if (id != MemDTO.memberid)
            {
                return "修改會員資料失敗!";
            }
            Member Mem = await _context.Members.FindAsync(id);
            if (Mem == null)
            {
                return "修改會員資料失敗!";
            }
            Mem.ImgName = MemDTO.memberimg;
            Mem.NickName = MemDTO.membernickname;
            Mem.RealName = MemDTO.membername;
            Mem.Phone = MemDTO.memberphone;
            Mem.Address = MemDTO.memberaddress;
            _context.Entry(Mem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
                {
                    return "修改會員資料失敗!";
                }
                else
                {
                    throw;
                }
            }

            return "修改會員資料成功!";
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
        [HttpPost]
        public async Task<MemberDTO> PostMember(MemberDTO MemDTO)
        {
            Member Mem = new Member
            {
                Id = -1,
                Phone = MemDTO.memberphone,
                Address = MemDTO.memberaddress,
                Email = MemDTO.memberemail,
                Password = MemDTO.memberpassword,
                Birthday = DateTime.Parse(MemDTO.memberbirth)

            };
            if (MemDTO.membergender == "男")
            {
                Mem.Gender = true;
            }
            else
            {
                Mem.Gender = false;
            }
            //密碼加密加鹽
            var (hashedPassword, salt) = _userService.HashPassword(MemDTO.memberaccount);

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
            if (member != null)
            {
                var hashedPassword = member.Password;
                var salt = member.Account; //Account是鹽
                var isPasswordValid = _userService.VerifyPassword(login.Password, hashedPassword, salt);

                if (isPasswordValid) 
                {
                    return Unauthorized(new { Message = "Invalid credentials"});
                }


                return Ok(new { Message = "Login successful"});
            }

            return Ok(new { Message = "查無此帳號"});
        }
    }
}
