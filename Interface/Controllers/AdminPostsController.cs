using Interface.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using source;
using Source.Models;

namespace Interface.Controllers
{
    [Authorize]
    public class AdminPostsController : Controller
    {
        private readonly ApplicationContext context;

        public AdminPostsController(ApplicationContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View(context.Posts.Include(p => p.Tags).Include(p => p.Category).Include(p => p.User).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewBag.Categories = await context.Categories
                                        .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                                        .ToListAsync();

            ViewBag.Tags = await context.Tags
                                        .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name })
                                        .ToListAsync();

            return View(new AddPostViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                string? imageUrl = null;
                if (model.Cover != null && model.Cover.Length > 0)
                {

                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/posts");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Cover.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Cover.CopyToAsync(stream);
                    }

                    imageUrl = "/uploads/posts/" + uniqueFileName;
                }

                var postTags = new List<Tag>();
                if (model.Tags != null && model.Tags.Count > 0)
                {
                    postTags = await context.Tags
                               .Where(t => model.Tags.Contains(t.Id))
                               .ToListAsync();
                }


                // Add new post
                if (model.Id == Guid.Empty)
                {
                    var newPost = new Post
                    {
                        CategoryId = model.CategoryId,
                        Title = model.Title,
                        Tags = postTags,
                        Content = model.Content,
                        UserId = "657b14f7-9aa8-412c-98f4-39412bdde8bf",
                        CreationDate = DateTime.Now,
                    };
                    if (!string.IsNullOrEmpty(imageUrl))
                        newPost.CoverUrl = imageUrl;

                    context.Posts.Add(newPost);

                }
                else
                {
                    // UPDATE EXISTING POST
                    var existingPost = await context.Posts
                        .Include(p => p.Tags)
                        .FirstOrDefaultAsync(p => p.Id == model.Id);

                    if (existingPost == null)
                        return NotFound();

                    existingPost.Title = model.Title;
                    existingPost.Content = model.Content;
                    existingPost.CategoryId = model.CategoryId;
                    existingPost.Tags = postTags;

                    if (imageUrl != null)
                    {
                        if (!string.IsNullOrEmpty(existingPost.CoverUrl))
                        {
                            var oldFilePath = Path.Combine(
                                Directory.GetCurrentDirectory(),
                                "wwwroot",
                                existingPost.CoverUrl.TrimStart('/')
                            );

                            if (System.IO.File.Exists(oldFilePath))
                                System.IO.File.Delete(oldFilePath);
                        }

                        existingPost.CoverUrl = imageUrl;
                    }
                    context.Posts.Update(existingPost);
                }
                await context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            ViewBag.Categories = await context.Categories
                                       .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                                       .ToListAsync();

            ViewBag.Tags = await context.Tags
                                        .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name })
                                        .ToListAsync();
            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            ViewBag.Categories = await context.Categories
                                        .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                                        .ToListAsync();

            ViewBag.Tags = await context.Tags
                                        .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name })
                                        .ToListAsync();

            var post = await context.Posts
                        .Where(p => p.Id == id)
                        .Select(p => new AddPostViewModel()
                        {
                            CategoryId = p.CategoryId,
                            Content = p.Content,
                            Id = id,
                            Tags = p.Tags.Select(t => t.Id).ToList(),
                            Title = p.Title,
                            ExistingCoverUrl = p.CoverUrl,
                        }).FirstOrDefaultAsync();


            if (post == null)
                return NotFound();

            return View("Add", post);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var post = await context.Posts
        .Include(p => p.Comments)
        .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
                return NotFound();

            if (!string.IsNullOrEmpty(post.CoverUrl) && post.CoverUrl != "/uploads/posts/Default.jpeg")
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", post.CoverUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            context.RemoveRange(post.Comments);
            context.Remove(post);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
