using Interface.Models;
using Interface.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PuppeteerSharp;
using source;
using Source.Models;
using System.Diagnostics;

namespace Interface.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext context;

        public HomeController(ILogger<HomeController> logger,ApplicationContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            if(User.Identity.IsAuthenticated)
                return RedirectToAction("Index","Post");

            var homePagePosts = await context.Posts.Include(p => p.User).Include(p => p.Tags)
                                                 .OrderByDescending(p => p.CreationDate)
                                                 .Take(3).ToListAsync();

            var homePagePostsModels = new List<PostViewModel>();
            foreach (var tagedPost in homePagePosts)
            {
                var post = MapToPostViewModel(tagedPost);
                homePagePostsModels.Add(post);
            }

            
            return View(homePagePostsModels);
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
