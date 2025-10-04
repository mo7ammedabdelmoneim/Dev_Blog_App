using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using source;
using Source.Models;

namespace Interface.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationContext context;

        public AccountController(UserManager<ApplicationUser> userManager, ApplicationContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        public IActionResult Index()
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = "Mohamed Ali";
            userManager.CreateAsync(user);

            context.SaveChanges();
            
            return Content("Successed");
        }

        // URL: /Account/CreateUser?username=ali
        public async Task<IActionResult> CreateUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest("Username is required.");
            string email = username + "@test.com";
            string password = "Test@123"; // باسورد ثابت للتجريب
            var existingUser = await userManager.FindByNameAsync(username);
            if (existingUser != null)
                return BadRequest("User already exists.");
            var user = new ApplicationUser
            {
                UserName = username,
                Email = email
            };
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return Ok("User created successfully with ID: " + user.Id);
            }
            return BadRequest("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
