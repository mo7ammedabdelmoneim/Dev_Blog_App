using Interface.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using source;
using Source.Models;

namespace Interface.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ApplicationContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public AdminController(ApplicationContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var totalUsers = await context.Users.CountAsync();
            var totalPosts = await context.Posts.CountAsync();
            var totalCategories = await context.Categories.CountAsync();
            var totalComments = await context.Comments.CountAsync();

            var lastestPosts = await context.Posts
                .Select(p => new LatestPostsDashboardViewModel
                {
                    Title = p.Title,
                    CreationDate = p.CreationDate
                })
                .OrderByDescending(p => p.CreationDate)
                .Take(5)
                .ToListAsync();

            var latestUsers = await context.Users
                .Select(u => new NewUsersDashboardViewModel
                {
                    Title = u.UserName,
                    CreationDate = u.CreatedAt,
                    Email = u.Email
                })
                .OrderByDescending(u => u.CreationDate)
                .Take(5)
                .ToListAsync();

            var roleNames = new[] { "admin", "manage_posts" };

            var postingUsers = await (
                from user in context.Users
                join userRole in context.UserRoles on user.Id equals userRole.UserId
                join role in context.Roles on userRole.RoleId equals role.Id
                where roleNames.Contains(role.Name)
                select user.Id
            ).Distinct().CountAsync();

            var avgPostsPerUser = totalPosts > 0 ? (double)postingUsers / totalPosts : 0;

            var mostActiveCategory = await context.Categories
                .Include(c => c.Posts)
                .OrderByDescending(c => c.Posts.Count)
                .FirstOrDefaultAsync();

            var mostReactedPost = await context.Posts
                .Include(p => p.PostReacts)
                .OrderByDescending(p => p.PostReacts.Count)
                .FirstOrDefaultAsync();

            var model = new AdminDashboardViewModel
            {
                TotalUsers = totalUsers,
                TotalPosts = totalPosts,
                TotalCategories = totalCategories,
                TotalComments = totalComments,
                LatestPosts = lastestPosts,
                LatestUsers = latestUsers,
                AvgPostsPerUser = avgPostsPerUser,
                MostActiveCategory = mostActiveCategory?.Name ?? "N/A",
                TopPostTitle = mostReactedPost?.Title ?? "N/A"
            };

            return View(model);
        }



    }
}
