using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using source;
using Source.Models;
using System.Security.Claims;

namespace Interface.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminCategoriesController : Controller
    {
        private readonly ApplicationContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public AdminCategoriesController(ApplicationContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var admin = await userManager.FindByEmailAsync(User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);
            var UserRole = await userManager.GetRolesAsync(admin);
            ViewBag.Role = UserRole.FirstOrDefault();

            return View(await context.Categories.Include(c => c.Posts).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category fromReq)
        {

            if (!ModelState.IsValid) { 
                
            }
            else
            {
                await context.AddAsync(fromReq);
                await context.SaveChangesAsync();
            }

            var admin = await userManager.FindByEmailAsync(User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);
            var UserRole = await userManager.GetRolesAsync(admin);
            ViewBag.Role = UserRole.FirstOrDefault();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await context.Categories
        .Include(c => c.Posts)
        .FirstOrDefaultAsync(c => c.Id == id);

            if (cat == null)
                return NotFound();

            if (cat.Name == "Uncategorized")
            {
                TempData["Error"] = "You cannot delete the default 'Uncategorized' category.";
                return RedirectToAction("Index");
            }

            var Uncategorized = await context.Categories
                .FirstOrDefaultAsync(c => c.Name == "Uncategorized");

            if (Uncategorized == null)
            {
                TempData["Error"] = "Default category 'Uncategorized' not found. Please create it first.";
                return RedirectToAction("Index");
            }
            context.Categories.Remove(cat);
            await context.SaveChangesAsync();

            var admin = await userManager.FindByEmailAsync(User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);
            var UserRole = await userManager.GetRolesAsync(admin);
            ViewBag.Role = UserRole.FirstOrDefault();

            return RedirectToAction(nameof(Index));

        }

    }
}
