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
    public class StaffsController : Controller
    {
        private readonly CharityContext _context;

        public StaffsController(CharityContext context)
        {
            _context = context;
        }


        // 顯示會員列表和進階搜尋
        public IActionResult Index(string gender, string name, DateTime? startDate, DateTime? endDate)
        {
            // 準備基本的會員查詢
            var query = _context.Staff.AsQueryable();

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

            // 根據到職日篩選
            if (startDate.HasValue)
            {
                query = query.Where(m => m.ArrivalDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(m => m.ArrivalDate <= endDate.Value);
            }

            // 返回篩選結果
            var members = query.ToList();
            return View(members);
        }

        //// GET: Staffs
        //public async Task<IActionResult> Index()
        //{
        //    var charityContext = _context.Staff.Include(s => s.AccessNavigation).Include(s => s.StatusNavigation);
        //    return View(await charityContext.ToListAsync());
        //}

        // GET: Staffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .Include(s => s.AccessNavigation)
                .Include(s => s.StatusNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // GET: Staffs/Create
        public IActionResult Create()
        {
            ViewData["Access"] = new SelectList(_context.StaffAccesses, "Id", "Id");
            ViewData["Status"] = new SelectList(_context.StaffStatuses, "Id", "Id");
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Account,Password,Name,RealName,Gender,Birthday,Email,Address,Phone,ArrivalDate,ResignDate,Status,Access")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                _context.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Access"] = new SelectList(_context.StaffAccesses, "Id", "Id", staff.Access);
            ViewData["Status"] = new SelectList(_context.StaffStatuses, "Id", "Id", staff.Status);
            return View(staff);
        }

        // GET: Staffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            ViewData["Access"] = new SelectList(_context.StaffAccesses, "Id", "Id", staff.Access);
            ViewData["Status"] = new SelectList(_context.StaffStatuses, "Id", "Id", staff.Status);
            return View(staff);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Account,Password,Name,RealName,Gender,Birthday,Email,Address,Phone,ArrivalDate,ResignDate,Status,Access")] Staff staff)
        {
            if (id != staff.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staff.Id))
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
            ViewData["Access"] = new SelectList(_context.StaffAccesses, "Id", "Id", staff.Access);
            ViewData["Status"] = new SelectList(_context.StaffStatuses, "Id", "Id", staff.Status);
            return View(staff);
        }

        // GET: Staffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .Include(s => s.AccessNavigation)
                .Include(s => s.StatusNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staff = await _context.Staff.FindAsync(id);
            if (staff != null)
            {
                _context.Staff.Remove(staff);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffExists(int id)
        {
            return _context.Staff.Any(e => e.Id == id);
        }
    }
}
