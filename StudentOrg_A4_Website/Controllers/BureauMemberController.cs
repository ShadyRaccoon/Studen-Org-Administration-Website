using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentOrg_A4_Website.Data;
using StudentOrg_A4_Website.Models;

namespace StudentOrg_A4_Website.Controllers
{
    public class BureauMemberController : Controller
    {
        private readonly StudentOrgContext _context;

        public BureauMemberController(StudentOrgContext context) 
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var bureau = await _context.BureauMembers
                .Include(b => b.Member)
                .Include(b => b.Position)
                .ToListAsync();

            return View(bureau);
        }
    }
}
