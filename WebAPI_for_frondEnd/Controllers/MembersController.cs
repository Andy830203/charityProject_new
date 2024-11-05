﻿using System;
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
using Microsoft.IdentityModel.Tokens;
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
        public async Task<IActionResult> PutMember1(int id, UpdateMember MemDTO)
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
            //Mem.ImgName = MemDTO.memberimg;
            Mem.NickName = MemDTO.Nickname;
            Mem.RealName = MemDTO.Name;
            Mem.Phone = MemDTO.Phone;
            Mem.Address = MemDTO.Address;
            Mem.Gender = MemDTO.Gender;
            Mem.Birthday = DateTime.Parse(MemDTO.Birth);
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

            return Ok(new { message = "修改會員資料成功!" }); ;
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
            var isPasswordValid = _userService.VerifyPassword(login.Password, hashedPassword, salt);

            if (!isPasswordValid)
            {
                return Unauthorized(new { Message = "密碼不正確" });
            }


            return Ok(new { Message = "登入成功",ID=ID, Name=Name});
        }
    }
}
