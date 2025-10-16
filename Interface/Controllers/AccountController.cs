using Interface.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using source;
using Source.Models;

namespace Interface.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationContext context;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, ApplicationContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
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

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Password does not match.");
                    return View(model);
                }

                var existingUser = await userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email already in use.");
                    return View(model);
                }

                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Post");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }

            return View(model);
        }
     
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await signInManager.PasswordSignInAsync(
                    user.UserName,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: true
                    );

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Post");
                    }

                    if (result.IsLockedOut)
                    {
                        ModelState.AddModelError("", "Account locked. Try again later.");
                        return View(model);
                    }
                }

                ModelState.AddModelError("", "Email or Password is incorrect.");
            }
            return View(model);
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
