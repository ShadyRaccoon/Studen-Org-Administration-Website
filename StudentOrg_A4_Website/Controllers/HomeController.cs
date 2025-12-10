using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StudentOrg_A4_Website.Data;
using StudentOrg_A4_Website.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentOrg_A4_Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly StudentOrgContext _context;

        public HomeController(ILogger<HomeController> logger, StudentOrgContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var recentPosts = await _context.Posts
                .OrderByDescending(p => p.PostDate)
                .Take(5)
                .ToListAsync();

            return View(recentPosts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
