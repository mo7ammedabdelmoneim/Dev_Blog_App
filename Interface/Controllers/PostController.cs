using Interface.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
using System.Security.Claims;

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
            //Categories DropDown
            ViewBag.Categories = context.Categories.ToList();

            var data = new PostIndexViewModel();

            // get most 3 featured posts
            var featuredPosts = await postService.GetFeaturedPosts();
            List<PostViewModel> featuredIndexPagePosts = new List<PostViewModel>();
            foreach (var post in featuredPosts)
            {
                var indexPagePost = await MapToPostViewModel(post);

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
                var indexPagePost = await MapToPostViewModel(post);

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

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var isReacted = await context.PostReactes.AnyAsync(pr => pr.PostId == post.Id && pr.UserId == userId);
            var postReacts = await context.PostReactes.CountAsync(pr => pr.PostId == post.Id);

            PostDetailsViewModel detailedPost = new PostDetailsViewModel();
            detailedPost.Post = await MapToPostViewModel(post);
            detailedPost.Comments = post.Comments;
            detailedPost.IsReacted = isReacted;

            return View(detailedPost);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> React(Guid postId)
        {
            var post = await context.Posts.FindAsync(postId);
            if (post == null)
                return NotFound();

            await context.AddAsync(new PostReacts
            {
                PostId = postId,
                UserId = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            });

            await context.SaveChangesAsync();

            var reactCount = await context.PostReactes.CountAsync(pc => pc.PostId == postId);

            return Json(new { success = true, newCount = reactCount });
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UnReact(Guid postId)
        {
            var userId = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var react = await context.PostReactes.FirstOrDefaultAsync(pc => pc.PostId == postId && pc.UserId == userId) ;
            if (react == null)
                return NotFound();


            context.Remove(react);

            await context.SaveChangesAsync();

            var reactCount = await context.PostReactes.CountAsync(pc=> pc.PostId == postId);

            return Json(new { success = true, newCount = reactCount });
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
                var post = await MapToPostViewModel(tagedPost);
                tagedPostsViewModel.Add(post);
            }

            PostByTagViewModel data = new PostByTagViewModel();
            data.Posts = tagedPostsViewModel;
            data.TagName = tag;
            data.CurrentPage = pageNumber;
            data.TotalPages = (int)Math.Ceiling(context.Posts.Where(p => p.Tags.Any(t => t.Name == tag)).Count() / (double)pageSize);
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
                var post = await MapToPostViewModel(catPost);
                categoryPostsViewModel.Add(post);
            }

            PostByCatViewModel data = new PostByCatViewModel();
            data.Posts = categoryPostsViewModel;
            data.CategoryName = category;
            data.CurrentPage = pageNumber;
            data.TotalPages = (int)Math.Ceiling(context.Posts.Where(p => p.Category.Name == category).Count() / (double)pageSize);
            return View(data);
        }


        public IActionResult add()
        {
            context.Add(new Comment
            {
                Id = Guid.NewGuid(),
                UserId = "0c5e4bab-71e7-4902-8dca-8ed359b1e793",
                PostId = context.Posts.First().Id,
                Content = "kbjkjdkjvfkjf"
            });

            context.SaveChanges();
            return Content("true");
        }

        private async Task<PostViewModel> MapToPostViewModel(Post post)
            {
            var reacts = await context.PostReactes.CountAsync(pr => pr.PostId == post.Id);
                return new PostViewModel
                {
                    PostId = post.Id,
                    Title = post.Title,
                    Content = post.Content,
                    AuthorName = post.User?.UserName,
                    CoverUrl = post.CoverUrl,
                    CreationDate = post.CreationDate,
                    Reacts = reacts,
                    Slug = post.Slug,
                    Tags = post.Tags
                };
            }
       


    }
}
