using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentOrg_A4_Website.Data;
using StudentOrg_A4_Website.Models;
using StudentOrg_A4_Website.ViewModels;
using System.Text.Json;

namespace StudentOrg_A4_Website.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly StudentOrgContext _context;

        public PostController(StudentOrgContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(CreatePostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Get username from session/identity
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                ModelState.AddModelError("", "Unable to determine logged-in user");
                return View(model);
            }

            // Check if the Google Drive ID exists in pictures table
            var bannerExists = await _context.Pictures
                .AnyAsync(p => p.Location == model.PostBanner);

            if (!bannerExists)
            {
                ModelState.AddModelError(nameof(model.PostBanner),
                    "The provided Google Drive ID does not exist in the database");
                return View(model);
            }

            // Create the post
            var post = new Post
            {
                PostTitle = model.PostTitle,
                PostDescription = model.PostDescription,
                PostContent = model.PostContent,
                PostBanner = model.PostBanner,
                PostAuthor = username
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Post created successfully!";
            return RedirectToAction("Index", "Posts");
        }
    }
}