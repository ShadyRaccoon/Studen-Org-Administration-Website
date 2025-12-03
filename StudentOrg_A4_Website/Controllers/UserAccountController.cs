using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using StudentOrg_A4_Website.Data;
using StudentOrg_A4_Website.Models;
using StudentOrg_A4_Website.Services;
using StudentOrg_A4_Website.ViewModels;

namespace StudentOrg_A4_Website.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly StudentOrgContext _context;
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly UserManager<UserAccount> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UserServices _userServices;

        public UserAccountController(
            StudentOrgContext context, 
            SignInManager<UserAccount> signInManager, 
            UserManager<UserAccount> userManager, 
            RoleManager<IdentityRole> roleManager, 
            UserServices userServices,
            IConfiguration configuration)
        {
            _userServices = userServices;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAdmin(string username, string email, string password) 
        {
            var user = new UserAccount
            {
                UserName = username,
                Email = email,
                IsActive = true,
                MemberId = null,
                CreationDate = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "admin");
                return RedirectToAction("Index", "Home");
            }

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAccount(int requestId)
        {
            var request = await _context.AccountRequests.FindAsync(requestId);
            if (request == null)
            {
                return BadRequest("Request not found.");
            }

            return View(requestId);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAccount(string email, string password, int requestId)
        {
            var request = await _context.AccountRequests.FindAsync(requestId);

            if (request == null)
            {
                return BadRequest("Request not found.");
            }

            var member = await _userServices.FindByName(request.RequestedFirstName, request.RequestedLastName);

            if (member == null)
            {
                return BadRequest("No member associated with the request");
            }

            var user = new UserAccount
            {
                Member = member,
                MemberId = member.MemberId,
                IsActive = true,
                UserName  = $"{member.FirstName.Trim()}{member.MemberId}{member.LastName.Trim()}",
                Email = email,
                CreationDate = DateTime.UtcNow
            };

            var userAccount = await _userManager.CreateAsync(user, password);

            if (userAccount.Succeeded)
            {
                request.RequestStatus = "Accepted";
                await _context.SaveChangesAsync();
                return RedirectToAction("PendingRequests","AccountRequest");
            }

            foreach (var error in userAccount.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(requestId);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.UsernameOrEmail) ?? await _userManager.FindByNameAsync(model.UsernameOrEmail);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty,"No such user found");
                return View(model);
            }

            if (!user.IsActive)
            {
                ModelState.AddModelError(string.Empty, "Your account has been deactivated");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent : false, lockoutOnFailure : true);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut) 
            { }

            return View();
        }

        public async Task<IActionResult> Logout() 
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool isOwner() => (User.Identity?.Name == _configuration["AdminOwnerUsername"]);

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var accounts = await _context.Users.ToListAsync();

            var models = new List<AccountRoleViewModel>();

            foreach (var account in accounts)
            {
                string? roleName = await (
                            from ur in _context.UserRoles
                            join r in _context.Roles
                            on ur.RoleId equals r.Id
                            where ur.UserId == account.Id
                            select r.Name
                        ).FirstOrDefaultAsync();

                models.Add(
                    new AccountRoleViewModel
                    {
                        Username = account.UserName,
                        Name = account.Member.LastName + account.Member.FirstName,
                        Role = roleName == null ? "null" : roleName
                    }
                    );
            }

            return View(accounts);
        }
    }
}