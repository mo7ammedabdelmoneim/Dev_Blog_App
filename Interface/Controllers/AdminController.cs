using Interface.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using source;
using Source.Models;

namespace Interface.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public AdminController(ApplicationContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [Authorize(Roles = "admin,manage_posts")]
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

            var admin = await userManager.FindByNameAsync(User?.Identity?.Name);
            var UserRole = await userManager.GetRolesAsync(admin);
            ViewBag.Role = UserRole.FirstOrDefault();

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Stats()
        {
            var monthlyUserGrowth = await context.Users
                .GroupBy(u => new { u.CreatedAt.Year, u.CreatedAt.Month })
                .Select(g => new GrowthItem
                {
                    Month = $"{g.Key.Month}/{g.Key.Year}",
                    Count = g.Count()
                })
                .ToListAsync();

            var monthlyPostGrowth = await context.Posts
                .GroupBy(p => new { p.CreationDate.Year, p.CreationDate.Month })
                .Select(g => new GrowthItem
                {
                    Month = $"{g.Key.Month}/{g.Key.Year}",
                    Count = g.Count()
                })
                .ToListAsync();

            var postsWithComments = await context.Posts
                .Include(p => p.Comments)
                .Include(p => p.PostReacts)
                .ToListAsync();

            double avgCommentsPerPost = postsWithComments.Any()
                ? postsWithComments.Average(p => p.Comments.Count)
                : 0;

            double avgReactsPerPost = postsWithComments.Any()
                ? postsWithComments.Average(p => p.PostReacts.Count)
                : 0;

            var topCategories = await context.Categories
                .Select(c => new CategoryItem
                {
                    Name = c.Name,
                    PostCount = c.Posts.Count()
                })
                .OrderByDescending(c => c.PostCount)
                .Take(5)
                .ToListAsync();

            var roleCounts = await (
                from role in context.Roles
                join userRole in context.UserRoles on role.Id equals userRole.RoleId
                group userRole by role.Name into g
                select new RoleItem
                {
                    RoleName = g.Key,
                    Count = g.Count()
                }
            ).ToListAsync();

            var model = new AdminStatsViewModel
            {
                MonthlyUserGrowth = monthlyUserGrowth,
                MonthlyPostGrowth = monthlyPostGrowth,
                AvgCommentsPerPost = avgCommentsPerPost,
                AvgReactsPerPost = avgReactsPerPost,
                TopCategories = topCategories,
                RoleCounts = roleCounts
            };

            var admin = await userManager.FindByNameAsync(User?.Identity?.Name);
            var UserRole = await userManager.GetRolesAsync(admin);
            ViewBag.Role = UserRole.FirstOrDefault();

            return View(model);
        }



    }
}
