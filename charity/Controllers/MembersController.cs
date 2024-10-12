using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using charity.Models;

namespace charity.Controllers
{
    [Authorize]
    public class MembersController : Controller
    {
        private readonly CharityContext _context;

        public MembersController(CharityContext context)
        {
            _context = context;
        }

        // 顯示會員列表和進階搜尋
        public IActionResult Index(string[] gender, string name, int?[] status, DateTime? startDate, DateTime? endDate)
        {
            // 準備基本的會員查詢
            var query = _context.Members.AsQueryable();

            // 根據姓名篩選
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(m => m.RealName.Contains(name));
            }

            // 根據性別篩選
            if (gender != null && gender.Length > 0)
            {
                var isMale = gender.Contains("male");
                var isFemale = gender.Contains("female");
                query = query.Where(m => (m.Gender == true && isMale) || (m.Gender == false && isFemale));
            }

            // 根據帳號狀態篩選 (1 正常, 2 停權)
            if (status != null && status.Length > 0)
            {
                query = query.Where(m => status.Contains(m.Status));
            }

            // 根據生日篩選
            if (startDate.HasValue)
            {
                query = query.Where(m => m.Birthday >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(m => m.Birthday <= endDate.Value);
            }

            // 返回篩選結果
            var members = query.ToList();
            return View(members);
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.AccessNavigation)
                .Include(m => m.StatusNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Account,Password,NickName,RealName,Gender,Birthday,Email,Address,Phone,Points,Checkin,Exp,ImgName,Status,Access,FaceRec")] Member member)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form.Files["ImgName"] != null)
                {
                    var file = Request.Form.Files["ImgName"];
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\members", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    member.ImgName = @"/images/members/" + fileName;
                }
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Account,Password,NickName,RealName,Gender,Birthday,Email,Address,Phone,Points,Checkin,Exp,ImgName,Status,Access,FaceRec")] Member member)
        {
            if (id != member.Id)
            {
                return NotFound();
            }

            var existingMember = await _context.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (existingMember == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Request.Form.Files["ImgName"] != null)
                    {
                        var file = Request.Form.Files["ImgName"];
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\members", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        member.ImgName = @"/images/members/" + fileName;
                    }
                    else
                    {
                        member.ImgName = existingMember.ImgName;
                    }
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.AccessNavigation)
                .Include(m => m.StatusNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Members/GetPicture/1
        public async Task<FileResult>? GetPicture(int id)
        {
            Member? m = await _context.Members.FindAsync(id);
            string? imagePath = m.ImgName;
            if (System.IO.File.Exists(imagePath))
            {
                string fileExtension = Path.GetExtension(imagePath).ToLower();
                string contentType;
                switch (fileExtension)
                {
                    case ".jpg":
                    case ".jpeg":
                        contentType = "image/jpeg";
                        break;
                    case ".png":
                        contentType = "image/png";
                        break;
                    case ".gif":
                        contentType = "image/gif";
                        break;
                    default:
                        contentType = "application/octet-stream";
                        break;
                }
                return File(System.IO.File.ReadAllBytes(imagePath), contentType);
            }
            else
            {
                return null;
            }
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }
    }
}
