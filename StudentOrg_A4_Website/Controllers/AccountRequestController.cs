using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentOrg_A4_Website.Data;
using StudentOrg_A4_Website.Models;


namespace StudentOrg_A4_Website.Controllers
{
    public class AccountRequestController : Controller
    {
        private readonly StudentOrgContext _context;

        public AccountRequestController(StudentOrgContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Bureau,Admin")]
        public IActionResult RequestAccount()
        {
            return View();
        }

        [HttpPost]
        [Authorize (Roles = "Bureau,Admin")]
        public async Task<IActionResult> RequestAccount(AccountRequest request)
        {
            request.RequestDate = DateOnly.FromDateTime(DateTime.Now);
            request.RequestStatus = "Pending";
            request.RequestAuthor = User.Identity.Name;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (user != null)
            {
                var userRole = await _context.UserRoles.FirstOrDefaultAsync(r => r.UserId == user.Id);
                if (userRole != null)     
                {
                    var role = await _context.Roles.FirstOrDefaultAsync(n => n.Id == userRole.RoleId);
                    if (role != null) 
                    {
                        request.RequestAuthorRole = role.Name;
                    }
                }
            }

            if (ModelState.IsValid)
            {
                try 
                {
                    _context.AccountRequests.Add(request);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("RequestAccount","AccountRequest");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("","Failed to submit request.");
                }

                return View(request);
            }

            return View();
        }
    }
}
