using Microsoft.AspNetCore.Mvc;
using StudentOrg_A4_Website.Data;
using StudentOrg_A4_Website.Models;

namespace StudentOrg_A4_Website.Controllers
{
    public class MemberController : Controller
    {
        private readonly StudentOrgContext _context;

        public MemberController(StudentOrgContext context) 
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult AddMember() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMember(Member member)
        {
            member.JoinDate = DateOnly.FromDateTime(DateTime.Now);

            if (ModelState.IsValid) {
                try
                {
                    _context.Members.Add(member);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("AddMember","Member");
                }
                catch (Exception ex) {
                    ModelState.AddModelError("","Failed to add member to the database.");
                }
            }
            return View(member);
        }
    }
}
