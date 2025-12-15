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
        private readonly UserManager<UserAccount> _userManager;

        public BureauMemberController(StudentOrgContext context, UserManager<UserAccount> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Get all positions
            var positions = await _context.BureauPositions.ToListAsync();

            // Get current bureau members (no end date)
            var currentMembers = await _context.BureauMembers
                .Include(bm => bm.Member)
                .Include(bm => bm.Position)
                .Where(bm => bm.EndTermDate == null)
                .ToListAsync();

            // Get past bureau members (has end date)
            var pastMembers = await _context.BureauMembers
                .Include(bm => bm.Member)
                .Include(bm => bm.Position)
                .Where(bm => bm.EndTermDate != null)
                .OrderByDescending(bm => bm.EndTermDate)
                .ToListAsync();

            // Build the list - one entry per position
            var bureauList = new List<BureauMember>();

            foreach (var position in positions)
            {
                var current = currentMembers.FirstOrDefault(bm => bm.PositionId == position.PositionId);

                if (current != null)
                {
                    bureauList.Add(current);
                }
                else
                {
                    // Vacant - create a placeholder
                    bureauList.Add(new BureauMember
                    {
                        PositionId = position.PositionId,
                        Position = position,
                        Member = null,  // vacant
                        StartTermDate = DateOnly.FromDateTime(DateTime.Now)
                    });
                }
            }

            // Append past members
            bureauList.AddRange(pastMembers);

            var membersList = await _context.Members
                .OrderBy(m => m.FirstName)
                .ThenBy(m => m.LastName)
                .Select(m => new SelectListItem
                {
                    Value = m.MemberId.ToString(),
                    Text = $"{m.FirstName} {m.LastName}"
                })
                .ToListAsync();

            // Add vacate option at the top
            membersList.Insert(0, new SelectListItem { Value = "0", Text = "— Vacate —" });

            ViewBag.Members = membersList;

            return View(bureauList);
        }
        
        [HttpGet]
        public async Task<IActionResult> CurrentBureau()
        {
            var bureau = await _context.BureauMembers
                .Include(b => b.Member)
                .Include(b => b.Position)
                .Where(b => b.EndTermDate == null)
                .OrderBy(b => b.PositionId)
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
            var oldHolderIds = new List<int>();

            foreach (var assignment in positionAssignments)
            {
                var positionId = assignment.Key;
                var newMemberId = assignment.Value;

                if (newMemberId == null) continue;

                // Handle vacate
                if (newMemberId == 0)
                {
                    var holder = await _context.BureauMembers
                        .FirstOrDefaultAsync(b => b.PositionId == positionId && b.EndTermDate == null);

                    if (holder != null)
                    {
                        holder.EndTermDate = DateOnly.FromDateTime(DateTime.Today);
                        oldHolderIds.Add(holder.MemberId);
                    }
                    continue;
                }

                var currentHolder = await _context.BureauMembers
                    .FirstOrDefaultAsync(b => b.PositionId == positionId && b.EndTermDate == null);

                if (currentHolder != null)
                {
                    if (currentHolder.MemberId == newMemberId) continue;

                    currentHolder.EndTermDate = DateOnly.FromDateTime(DateTime.Today);
                    oldHolderIds.Add(currentHolder.MemberId);
                }

                // Load the member first
                var member = await _context.Members.FindAsync(newMemberId.Value);

                var newBureauMember = new BureauMember
                {
                    PositionId = positionId,
                    MemberId = newMemberId.Value,
                    Member = member,  // <-- Add this line
                    StartTermDate = DateOnly.FromDateTime(DateTime.Today),
                    EndTermDate = null
                };
                _context.BureauMembers.Add(newBureauMember);

                await UpdateUserRole(newMemberId.Value, true);
            }

            await _context.SaveChangesAsync();

            foreach (var memberId in oldHolderIds)
            {
                await UpdateUserRole(memberId, false);
            }

            return RedirectToAction(nameof(Index));
        }
        private async Task UpdateUserRole(int memberId, bool addRole)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.MemberId == memberId);

            if (user == null) return;

            if (addRole)
            {
                if (!await _userManager.IsInRoleAsync(user, "Bureau"))
                {
                    await _userManager.AddToRoleAsync(user, "Bureau");
                }
            }
            else
            {
                var stillInBureau = await _context.BureauMembers
                    .AnyAsync(b => b.MemberId == memberId && b.EndTermDate == null);

                if (!stillInBureau && await _userManager.IsInRoleAsync(user, "Bureau"))
                {
                    await _userManager.RemoveFromRoleAsync(user, "Bureau");
                }
            }
        }
    }
}