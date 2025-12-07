using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentOrg_A4_Website.Data;
using StudentOrg_A4_Website.Models;

namespace StudentOrg_A4_Website.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly StudentOrgContext _context;

        public DepartmentController(StudentOrgContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments
                .Include(d => d.Members)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            return View(departments);
        }

        [HttpGet]
        public async Task<IActionResult> AddMember(int id)
        {
            var department = await _context.Departments
                .Include(d => d.Members)
                .FirstOrDefaultAsync(d => d.DepartmentId == id);

            if (department == null) return NotFound();

            var memberIdsInDept = department.Members.Select(m => m.MemberId).ToList();

            var availableMembers = await _context.Members
                .Where(m => !memberIdsInDept.Contains(m.MemberId) && m.LeaveDate == null)
                .OrderBy(m => m.FirstName)
                .ThenBy(m => m.LastName)
                .ToListAsync();

            ViewBag.Department = department;
            return View(availableMembers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMember(int departmentId, int memberId)
        {
            var department = await _context.Departments
                .Include(d => d.Members)
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);

            var member = await _context.Members.FindAsync(memberId);

            if (department == null || member == null) return NotFound();

            if (!department.Members.Contains(member))
            {
                department.Members.Add(member);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(AddMember), new { id = departmentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMember(int departmentId, int memberId)
        {
            var department = await _context.Departments
                .Include(d => d.Members)
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);

            if (department == null) return NotFound();

            var member = department.Members.FirstOrDefault(m => m.MemberId == memberId);

            if (member != null)
            {
                department.Members.Remove(member);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}