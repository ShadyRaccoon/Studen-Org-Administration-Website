using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentOrg_A4_Website.Data;
using StudentOrg_A4_Website.Models;
using StudentOrg_A4_Website.Services;
using StudentOrg_A4_Website.ViewModels;

namespace StudentOrg_A4_Website.Controllers
{
    public class BureauMemberController : Controller
    {
        private readonly StudentOrgContext _context;
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly UserManager<UserAccount> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public BureauMemberController() { }
    }
}
