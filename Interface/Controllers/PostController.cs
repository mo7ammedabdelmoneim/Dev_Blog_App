using Interface.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public PostController(ApplicationContext context, UserManager<ApplicationUser> userManager,IPostService postService)
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
        public async Task<IActionResult> ByTag(string tag,int pageNumber=1)
        {
            if (string.IsNullOrWhiteSpace(tag))
                return RedirectToAction("Index");

            if (pageNumber < 1)
                pageNumber = 1;
            const int pageSize = 9;
            var tagedPosts = await postService.GetByTagPosts(tag,pageSize,pageNumber);
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
            data.Posts = tagedPostsViewModel ;
            data.TagName = tag;
            data.CurrentPage = pageNumber;
            data.TotalPages = (int)Math.Ceiling(tagedPosts.Count / (double)pageSize);
            return View(data);
        }
        
        [HttpGet]
        public async Task<IActionResult> ByCategory(string category, int pageNumber=1)
        {
            if (string.IsNullOrWhiteSpace(category))
                return RedirectToAction("Index");

            if (pageNumber < 1)
                pageNumber = 1;
            const int pageSize = 9;

            var postsOfCategory = await postService.GetByCategoryPosts(category,pageSize,pageNumber);
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

        public async Task<IActionResult> Add()
        {
            var posts = await context.Posts.Take(5).ToListAsync();
            var users = await context.Users.Take(5).ToListAsync();

            var reacts = new List<PostReacts>
            {
                new PostReacts { Id = Guid.NewGuid(), PostId = posts[0].Id, UserId = users[0].Id },
                new PostReacts { Id = Guid.NewGuid(), PostId = posts[0].Id, UserId = users[1].Id },
                new PostReacts { Id = Guid.NewGuid(), PostId = posts[1].Id, UserId = users[2].Id },
                new PostReacts { Id = Guid.NewGuid(), PostId = posts[1].Id, UserId = users[3].Id },
                new PostReacts { Id = Guid.NewGuid(), PostId = posts[2].Id, UserId = users[4].Id },
                new PostReacts { Id = Guid.NewGuid(), PostId = posts[3].Id, UserId = users[1].Id },
                new PostReacts { Id = Guid.NewGuid(), PostId = posts[3].Id, UserId = users[2].Id },
                new PostReacts { Id = Guid.NewGuid(), PostId = posts[4].Id, UserId = users[0].Id },
                new PostReacts { Id = Guid.NewGuid(), PostId = posts[4].Id, UserId = users[3].Id }
            };

            await context.PostReactes.AddRangeAsync(reacts);
            await context.SaveChangesAsync();

            return Ok("Sample reacts seeded successfully!");
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
