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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 9;

            var posts = await _context.Posts
                .OrderByDescending(p => p.PostDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPosts = await _context.Posts.CountAsync();
            var totalPages = (int)Math.Ceiling(totalPosts / (double)pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(posts);
        }
    }
}
