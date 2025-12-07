using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentOrg_A4_Website.Data;

namespace StudentOrg_A4_Website.Controllers
{
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
            var departmentMembers = await _context.Departments
                .Include(d => d.Members)
                .Include(d => d.DepartmentName)
                .ToListAsync();

            return View(departmentMembers);
        }
    }
}
