using Google.Apis.Drive.v3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentOrg_A4_Website.Data;
using StudentOrg_A4_Website.Models;
using StudentOrg_A4_Website.Services;
using StudentOrg_A4_Website.ViewModels;
using System.Text.Json;

namespace StudentOrg_A4_Website.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly StudentOrgContext _context;
        private readonly GoogleDriveServices _service;

        public PostController(StudentOrgContext context, GoogleDriveServices service)
        {
            _context = context;
            _service = service;
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
        [AllowAnonymous]
        public async Task<IActionResult> PublicPosts()
        {
            var posts = await _context.Posts
                .OrderByDescending(m => m.PostDate)
                .ToListAsync();

            return View(posts);
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            var json = TempData.Peek("PreviewPost") as string;
            if (json != null)
            {
                var model = JsonSerializer.Deserialize<PostViewModel>(json);
                return View(model);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(PostViewModel model)
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
            try
            {
                var fileExists = await _service.FileExistsAsync(model.PostBanner);
                if (!fileExists)
                {
                    ModelState.AddModelError(nameof(model.PostBanner),
                        "The provided Google Drive ID is not valid or accessible");
                    return View(model);
                }

                var alreadyExists = await _context.Pictures.AnyAsync(p => p.Location == model.PostBanner);
                if (!alreadyExists)
                {
                    var picture = new Picture
                    {
                        Location = model.PostBanner
                    };
                    await _context.Pictures.AddAsync(picture);
                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
                ModelState.AddModelError(nameof(model.PostBanner),
                    "Could not validate the Google Drive ID");
                return View(model);
            }

            model.PostAuthor = username;
            model.PostDate = DateOnly.FromDateTime(DateTime.Now);

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

            var model = new PostViewModel 
            { 
                PostId = id,
                PostBanner = post.PostBanner,
                PostTitle = post.PostTitle,
                PostAuthor = post.PostAuthor,
                PostDescription = post.PostDescription,
                PostContent = post.PostContent,
                PostDate = post.PostDate
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int id, PostViewModel model)
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

            var model = JsonSerializer.Deserialize<PostViewModel>(json);
            TempData.Keep("PreviewPost");
            TempData.Keep("Origin");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PublishPost()
        {
            var json = TempData["PreviewPost"] as string;
            var origin = TempData["Origin"] as string;
            if (json == null)
            {
                return RedirectToAction("CreatePost");
            }

            var model = JsonSerializer.Deserialize<PostViewModel>(json);

            TempData.Remove("PreviewPost");
            TempData.Remove("Origin");

            if (origin == "EditPost")
            {
                var existingPost = await _context.Posts.FindAsync(model.PostId);

                if (existingPost != null)
                {
                    existingPost.PostBanner = model.PostBanner;
                    existingPost.PostTitle = model.PostTitle;
                    existingPost.PostDescription = model.PostDescription;
                    existingPost.PostContent = model.PostContent;
                }

                _context.Posts.Update(existingPost);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Post updated!";

                return RedirectToAction("Index");

            }
            else if (origin == "CreatePost")
            {
                var post = new Post
                {
                    PostAuthor = model.PostAuthor,
                    PostBanner = model.PostBanner,
                    PostContent = model.PostContent,
                    PostTitle = model.PostTitle,
                    PostDescription = model.PostDescription,
                    PostDate = model.PostDate
                };

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Post published!";

                return RedirectToAction("CreatePost");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemovePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return BadRequest();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DetailedView(int id)
        { 
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == id);

            if (post == null)
            { 
                return NotFound();
            }
            return View(post);
        }
    }
}