using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using charity.Models;
using Microsoft.AspNetCore.Authorization;

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
        public IActionResult Index(string gender, string name, int? status,  DateTime? startDate, DateTime? endDate)
        {
            // 準備基本的會員查詢
            var query = _context.Members.AsQueryable();

            // 根據姓名篩選
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(m => m.RealName.Contains(name));
            }

            // 根據性別篩選
            if (!string.IsNullOrEmpty(gender))
            {
                query = query.Where(m => m.Gender == (gender == "male"));
            }

            // 根據帳號狀態篩選 (1 正常, 2 停權)
            if (status.HasValue)
            {
                query = query.Where(m => m.Status == status.Value);
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

        // GET: Members
        //public async Task<IActionResult> Index()
        //{
        //    var charityContext = _context.Members.Include(m => m.AccessNavigation).Include(m => m.StatusNavigation);
        //    return View(await charityContext.ToListAsync());
        //}

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
            //ViewData["Access"] = new SelectList(_context.MemberAccesses, "Id", "Id");
            //ViewData["Status"] = new SelectList(_context.MemberStatuses, "Id", "Id");
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Account,Password,NickName,RealName,Gender,Birthday,Email,Address,Phone,Points,Checkin,Exp,ImgName,Status,Access,FaceRec")] Member member)
        {
            if (ModelState.IsValid)  //因為一開始沒有登入所以驗證false走不進去
            {
                if (Request.Form.Files["ImgName"] != null)//&& MemberPhoto.Length > 0
                {
                    var file = Request.Form.Files["ImgName"];
                    // 獲取文件的名稱
                    var fileName = Path.GetFileName(file.FileName);

                    // 定義儲存照片的路徑 (例如 wwwroot/images/members)
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\members", fileName);

                    // 保存文件到指定路徑
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // 將文件名保存到資料庫
                    member.ImgName = @"/images/members/" + fileName;
                }
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["Access"] = new SelectList(_context.MemberAccesses, "Id", "Id", member.Access);
            //ViewData["Status"] = new SelectList(_context.MemberStatuses, "Id", "Id", member.Status);
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
            ViewData["Access"] = new SelectList(_context.MemberAccesses, "Id", "Id", member.Access);
            ViewData["Status"] = new SelectList(_context.MemberStatuses, "Id", "Id", member.Status);
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Account,Password,NickName,RealName,Gender,Birthday,Email,Address,Phone,Points,Checkin,Exp,ImgName,Status,Access,FaceRec")] Member member)
        {
            if (id != member.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid) //因為一開始沒有登入所以驗證false走不進去
            {
                try
                {
                    if (Request.Form.Files["ImgName"] != null) //&& MemberPhoto.Length > 0
                    {
                        var file = Request.Form.Files["ImgName"];
                        // 獲取文件的名稱
                        var fileName = Path.GetFileName(file.FileName);

                        // 定義儲存照片的路徑 (例如 wwwroot/images/members)
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\members", fileName);

                        // 保存文件到指定路徑
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // 將文件名保存到資料庫
                        member.ImgName = @"/images/members/" + fileName;
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
            ViewData["Access"] = new SelectList(_context.MemberAccesses, "Id", "Id", member.Access);
            ViewData["Status"] = new SelectList(_context.MemberStatuses, "Id", "Id", member.Status);
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

        //GET: /Members/GetPicture/1
        public async Task<FileResult>? GetPicture(int id)
        {
            Member? m = await _context.Members.FindAsync(id);
            string? imagePath = m.ImgName;
            if (System.IO.File.Exists(imagePath))
            {
                // Get the file extension to determine the content type
                string fileExtension = Path.GetExtension(imagePath).ToLower(); //讀副檔名

                // Set the MIME type based on the file extension
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
                        contentType = "application/octet-stream"; // generic binary data
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
