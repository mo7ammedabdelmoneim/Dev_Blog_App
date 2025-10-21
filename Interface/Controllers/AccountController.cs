using Interface.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using source;
using Source.Models;
using System.Security.Claims;

namespace Interface.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationContext context;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, ApplicationContext context, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
            this.roleManager = roleManager;
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
                    var roleResult = await userManager.AddToRoleAsync(user, "user");
                    if (roleResult.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Post");
                    }
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }

            return View(model);
        }
     
        [HttpGet]
        public async Task<IActionResult> Logout()
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
                        if (User.IsInRole("admin") || User.IsInRole("manage_posts"))
                            return RedirectToAction("Index", "Admin");

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

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            if (remoteError != null)
            {
                TempData["ErrorMessage"] = $"Error from external provider: {remoteError}";
                return RedirectToAction("Login");
            }

            // Get Login Data from Google
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["ErrorMessage"] = "Error loading external login information.";
                return RedirectToAction("Login");
            }

            // Try to sign in directly if user already linked before
            var signInResult = await signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true
            );

            if (signInResult.Succeeded)
            {
                return RedirectToAction("Index", "Post");
            }

            // Extract email from Google
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (email == null)
            {
                TempData["ErrorMessage"] = "Email not provided by Google.";
                return RedirectToAction("Login");
            }

            // Check if user already exists
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                // Safe username 
                var safeUserName = email.Split('@')[0];

                user = new ApplicationUser
                {
                    UserName = safeUserName,
                    Email = email,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    TempData["ErrorMessage"] = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    return RedirectToAction("Login");
                }

                var roleResult= await userManager.AddToRoleAsync(user,"user");
                if (!roleResult.Succeeded)
                {
                    TempData["ErrorMessage"] = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    return RedirectToAction("Login");
                }
            }

            // Link Google account with the user 
            var addLoginResult = await userManager.AddLoginAsync(user, info);
            if (!addLoginResult.Succeeded)
            {
                TempData["ErrorMessage"] = string.Join(", ", addLoginResult.Errors.Select(e => e.Description));
                return RedirectToAction("Login");
            }

            // Actual sign-in
            await signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index", "Post");
        }
    }
}
