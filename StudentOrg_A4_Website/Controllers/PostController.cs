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
        public async Task<IActionResult> Index() 
        {
            var posts = await _context.Posts
                .OrderBy(m => m.PostDate)
                .ToListAsync();

            return View(posts);
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            var json = TempData.Peek("PreviewPost") as string;
            if (json != null)
            {
                var model = JsonSerializer.Deserialize<CreatePostViewModel>(json);
                return View(model);
            }

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

            model.PostAuthor = username;

            TempData["PreviewPost"] = JsonSerializer.Serialize(model);
            TempData["Origin"] = "CreatePost";
            return RedirectToAction("PreviewPost");
        }

        //check in the db for posts and generate a view with the form fields filled 
        [HttpGet]
        public async Task<IActionResult> EditPost(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null) 
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int id, CreatePostViewModel model)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return BadRequest();
            }

            model.PostId = post.PostId;
            model.PostAuthor = post.PostAuthor;

            TempData["PreviewPost"] = JsonSerializer.Serialize(model);
            TempData["Origin"] = "EditPost";
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

        [HttpPost]
        public async Task<IActionResult> PublishPost()
        {
            var json = TempData["PreviewPost"] as string;
            if (json == null)
            {
                return RedirectToAction("CreatePost");
            }

            var model = JsonSerializer.Deserialize<CreatePostViewModel>(json);

            var post = new Post
            {
                PostAuthor = model.PostAuthor, 
                PostBanner = model.PostBanner,
                PostContent = model.PostContent,
                PostTitle = model.PostTitle,
                PostDescription = model.PostDescription,
                PostId = model.PostId
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Post published!";

            return RedirectToAction("CreatePost");
        }
        
    }
}