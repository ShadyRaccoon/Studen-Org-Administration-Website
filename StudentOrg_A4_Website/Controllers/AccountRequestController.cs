using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Storage.Internal.Json;
using StudentOrg_A4_Website.Data;
using StudentOrg_A4_Website.Models;
using StudentOrg_A4_Website.Services;

namespace StudentOrg_A4_Website.Controllers
{
    public class AccountRequestController : Controller
    {
        private readonly StudentOrgContext _context;
        private readonly UserServices _userServicesProvider;
        public AccountRequestController(StudentOrgContext context, UserServices userServicesProvider)
        {
            _context = context;
            _userServicesProvider = userServicesProvider;
        }

        [HttpGet]
        [Authorize(Roles = "Bureau,Admin")]
        public IActionResult RequestAccount()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Bureau,Admin")]
        public async Task<IActionResult> RequestAccount(AccountRequest request)
        {
            request.RequestDate = DateOnly.FromDateTime(DateTime.Now);
            request.RequestStatus = "Pending";

            var memberExists = await _context.Members.AnyAsync(m =>
            m.FirstName == request.RequestedFirstName &&
            m.LastName == request.RequestedLastName);

            if (!memberExists)
            {
                ModelState.AddModelError("", "Member not found in system.");
                return View(request);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user != null)
            {
                var member = await _context.Members.FirstOrDefaultAsync(m => m.MemberId == user.MemberId);
                if (member != null)
                {
                    request.RequestAuthor = $"{member.FirstName} {member.LastName}";
                }
                else
                {
                    request.RequestAuthor = user.UserName;
                }

                var role = await _context.UserRoles.FirstOrDefaultAsync(r => r.UserId == user.Id);
                if (role != null)
                {
                    var roleName = await _context.Roles.FirstOrDefaultAsync(rn => rn.Id == role.RoleId);
                    if (roleName != null)
                    {
                        request.RequestAuthorRole = roleName.Name;
                    }
                }
            }

            ModelState.Remove("RequestAuthor");
            ModelState.Remove("RequestStatus");
            ModelState.Remove("RequestAuthorRole");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.AccountRequests.Add(request);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("RequestAccount", "AccountRequest");
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine($"Exception: {e.Message}");
                    ModelState.AddModelError("", $"Failed: {e.Message}");
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    System.Diagnostics.Debug.WriteLine($"ModelState Error: {error.ErrorMessage}");
                }
            }

            return View(request);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveRequest(int requestId) 
        {
            var request = await _context.AccountRequests.FindAsync(requestId);

            if (request != null)
            {
                request.RequestStatus = "Accepted";
                await _context.SaveChangesAsync();

                return RedirectToAction("CreateAccount", "UserAccount", new { requestId = request.RequestId});
            }

            return BadRequest("Request not found");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DenyRequest(int requestId)
        {
            var request = await _context.AccountRequests.FindAsync(requestId);

            if (request != null)
            {
                request.RequestStatus = "Denied";
                await _context.SaveChangesAsync();
                return RedirectToAction("PendingRequests");
            }

            return View();
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PendingRequests()
        {
            var requests = await _context.AccountRequests
                .Where(r => r.RequestStatus == "Pending")
                .ToListAsync();

            return View(requests);
        }
    }
}
