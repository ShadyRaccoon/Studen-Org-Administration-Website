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
            System.Diagnostics.Debug.WriteLine($"=== CreatePost POST ===");
            System.Diagnostics.Debug.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                if (state.Errors.Count > 0)
                {
                    foreach (var error in state.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error in {key}: {error.ErrorMessage}");
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine($"Username: {User.Identity?.Name}");

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

            model.PostAuthor = username;

            TempData["PreviewPost"] = JsonSerializer.Serialize(model);
            return RedirectToAction("PreviewPost");
        }

        [HttpGet]
        public IActionResult PreviewPost()
        {
            var json = TempData["PreviewPost"] as string;

            if (json == null)
            {
                return RedirectToAction("CreatePost");
            }

            var model = JsonSerializer.Deserialize<CreatePostViewModel>(json);
            TempData.Keep("PreviewPost");

            return View(model);
        }
    }
}