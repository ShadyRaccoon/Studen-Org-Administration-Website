using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentOrg_A4_Website.Data;
using StudentOrg_A4_Website.Models;

namespace StudentOrg_A4_Website.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly StudentOrgContext _context;

        public MemberController(StudentOrgContext context) 
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var members = await _context.Members
                .OrderBy(m => m.LeaveDate.HasValue)
                .ThenBy(m => m.LastName)
                .ThenBy(m => m.FirstName)
                .ThenByDescending(m => m.JoinDate)
                .ToListAsync();

            return View(members);   
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

        [HttpGet]
        public async Task<IActionResult> EditMember(int id)
        {
            var member = await _context.Members.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> EditMember(int id, Member member)
        {
            if (id != member.MemberId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Members.AnyAsync( e => e.MemberId == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Failed to update member.");
                }
            }

            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveMember(int id)
        {
            var member = await _context.Members.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            try
            {
                member.LeaveDate = DateOnly.FromDateTime(DateTime.Now);
                _context.Update(member);

                var userAccount = await _context.Users
                    .FirstOrDefaultAsync(u => u.MemberId == id);

                if (userAccount != null)
                {
                    userAccount.IsActive = false;
                    userAccount.TerminationDate = DateTime.Now;
                    _context.Update(userAccount);
                }

                var bureauMember = await _context.BureauMembers
                    .FirstOrDefaultAsync(bm => bm.MemberId == id && bm.EndTermDate == null);

                if (bureauMember != null)
                {
                    bureauMember.EndTermDate = DateOnly.FromDateTime(DateTime.Now);
                    _context.Update(bureauMember);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to remove member.";
                return RedirectToAction("Index");
            }
        }

    }
}
