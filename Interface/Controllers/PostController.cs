using Interface.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using source;
using Source.Models;
using Source.Services.Implementations;
using Source.Services.Interfaces;
using System.Linq;

namespace Interface.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPostService postService;

        public PostController(ApplicationContext context, UserManager<ApplicationUser> userManager, IPostService postService)
        {
            this.context = context;
            this.userManager = userManager;
            this.postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            var data = new PostIndexViewModel();

            // get most 3 featured posts
            var featuredPosts = await postService.GetFeaturedPosts();
            List<PostViewModel> featuredIndexPagePosts = new List<PostViewModel>();
            foreach (var post in featuredPosts)
            {
                var indexPagePost = MapToPostViewModel(post);

                featuredIndexPagePosts.Add(indexPagePost);
            }
            data.FeaturedPosts = featuredIndexPagePosts;


            // get most latest posts with pagnation
            if (pageNumber < 1)
                pageNumber = 1;
            const int pageSize = 9;

            var latestPosts = await postService.GetLatestPosts(pageSize, pageNumber);

            List<PostViewModel> latestIndexPagePosts = new List<PostViewModel>();
            foreach (var post in latestPosts)
            {
                var indexPagePost = MapToPostViewModel(post);

                latestIndexPagePosts.Add(indexPagePost);
            }
            data.LatestPosts = latestIndexPagePosts;

            data.CurrentPage = pageNumber;
            data.TotalPages = Math.Max(1, (int)Math.Ceiling(context.Posts.Count() / (double)9));

            return View(data);

        }

        [HttpGet]
        public async Task<IActionResult> Details(string slug)
        {
            var post = await postService.GetBySlug(slug);
            if (post == null)
            {
                return NotFound();
            }

            PostDetailsViewModel detailedPost = new PostDetailsViewModel();
            detailedPost.Post = MapToPostViewModel(post);
            detailedPost.Comments = post.Comments;
            return View(detailedPost);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleReact(Guid postId)
        {
            var post = await postService.GetById(postId);
            if (post == null)
                return NotFound();

            post.Reacts += 1;

            await postService.Save();

            return Json(new { success = true, reacts = post.Reacts });
        }

        [HttpGet]
        public async Task<IActionResult> ByTag(string tag, int pageNumber = 1)
        {
            if (string.IsNullOrWhiteSpace(tag))
                return RedirectToAction("Index");

            if (pageNumber < 1)
                pageNumber = 1;
            const int pageSize = 9;
            var tagedPosts = await postService.GetByTagPosts(tag, pageSize, pageNumber);
            if (tagedPosts == null)
            {
                return NotFound();
            }

            var tagedPostsViewModel = new List<PostViewModel>();
            foreach (var tagedPost in tagedPosts)
            {
                var post = MapToPostViewModel(tagedPost);
                tagedPostsViewModel.Add(post);
            }

            PostByTagViewModel data = new PostByTagViewModel();
            data.Posts = tagedPostsViewModel;
            data.TagName = tag;
            data.CurrentPage = pageNumber;
            data.TotalPages = (int)Math.Ceiling(tagedPosts.Count / (double)pageSize);
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> ByCategory(string category, int pageNumber = 1)
        {
            if (string.IsNullOrWhiteSpace(category))
                return RedirectToAction("Index");

            if (pageNumber < 1)
                pageNumber = 1;
            const int pageSize = 9;

            var postsOfCategory = await postService.GetByCategoryPosts(category, pageSize, pageNumber);
            if (postsOfCategory == null)
            {
                return NotFound();
            }

            var categoryPostsViewModel = new List<PostViewModel>();
            foreach (var catPost in postsOfCategory)
            {
                var post = MapToPostViewModel(catPost);
                categoryPostsViewModel.Add(post);
            }

            PostByTagViewModel data = new PostByTagViewModel();
            data.Posts = categoryPostsViewModel;
            data.TagName = category;
            data.CurrentPage = pageNumber;
            data.TotalPages = (int)Math.Ceiling(postsOfCategory.Count / (double)pageSize);
            return View(data);
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

            return View("Add",post);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var post = await context.Posts
        .Include(p => p.Comments)
        .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
                return NotFound();

            if (!string.IsNullOrEmpty(post.CoverUrl))
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


        private PostViewModel MapToPostViewModel(Post post)
            {
                return new PostViewModel
                {
                    PostId = post.Id,
                    Title = post.Title,
                    Content = post.Content,
                    AuthorName = post.User?.UserName,
                    CoverUrl = post.CoverUrl,
                    CreationDate = post.CreationDate,
                    Reacts = post.Reacts,
                    Slug = post.Slug,
                    Tags = post.Tags
                };
            }
       


    }
}
