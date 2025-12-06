using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                .OrderByDescending(b => b.EndTermDate == null) 
                .ThenBy(b => b.PositionId)
                .ToListAsync();

            ViewBag.Members = await _context.Members
                .OrderBy(m => m.FirstName)
                .ThenBy(m => m.LastName)
                .Select(m => new SelectListItem
                {
                    Value = m.MemberId.ToString(),
                    Text = $"{m.FirstName} {m.LastName}"
                })
                .ToListAsync();

            return View(bureau);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateBureau(Dictionary<int, int?> positionAssignments)
        { 
            foreach (var assignment in positionAssignments)
            {
                var positionId = assignment.Key;
                var newMemberId = assignment.Value;

                if (newMemberId == null) continue;

                var currentHolder = await _context.BureauMembers
                    .FirstOrDefaultAsync(b => b.PositionId == positionId && b.EndTermDate == null);

                if (currentHolder != null)
                {
                    if (currentHolder.MemberId == newMemberId) continue;

                    currentHolder.EndTermDate = DateOnly.FromDateTime(DateTime.Today);
                }

                var newBureauMember = new BureauMember
                {
                    PositionId = positionId,
                    MemberId = newMemberId.Value,
                    StartTermDate = DateOnly.FromDateTime(DateTime.Today),
                    EndTermDate = null
                };

                _context.BureauMembers.Add(newBureauMember);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
