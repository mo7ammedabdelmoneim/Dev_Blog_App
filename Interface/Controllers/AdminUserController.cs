using Interface.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using source;
using Source.Models;
using System;
using System.Security.Claims;

namespace Interface.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminUsersController : Controller
    {
        private readonly ApplicationContext context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminUsersController(ApplicationContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(string search, string role)
        {
            var users = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                users = users.Where(u => u.UserName.Contains(search) || u.Email.Contains(search));

            var userList = await users
                .OrderByDescending(u => u.CreatedAt)
                .Select(u => new AdminUserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    CreatedAt = u.CreatedAt
                }).ToListAsync();

           
            if (!string.IsNullOrEmpty(role))
            {
                var userIds = await (from ur in context.UserRoles
                                     join r in context.Roles on ur.RoleId equals r.Id
                                     where r.Name == role
                                     select ur.UserId).ToListAsync();

                userList = userList.Where(u => userIds.Contains(u.Id)).ToList();
            }

            
            foreach (var user in userList)
            {
                var roles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id));
                user.CurrentRole = roles.FirstOrDefault() ?? "None";
            }

            ViewBag.Roles = _roleManager.Roles.Select(r => r.Name).ToList();

            var admin = await _userManager.FindByEmailAsync(User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);
            var UserRole = await _userManager.GetRolesAsync(admin);
            ViewBag.Role = UserRole.FirstOrDefault();

            return View(userList);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(string userId, string newRole)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(newRole))
            {
                TempData["Message"] = "Invalid data provided.";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Message"] = "User not found.";
                return RedirectToAction(nameof(Index));
            }

            var oldRoles = await _userManager.GetRolesAsync(user);

            if (oldRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, oldRoles);
                if (!removeResult.Succeeded)
                {
                    TempData["Message"] = "Failed to remove old roles.";
                    return RedirectToAction(nameof(Index));
                }
            }

            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            if (!addResult.Succeeded)
            {
                TempData["Message"] = "Failed to add the new role.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Message"] = $" Role updated successfully for {user.UserName}!";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                var userPosts = await context.Posts.Where(p => p.UserId == user.Id).ToListAsync();
                context.Posts.RemoveRange(userPosts);
                await context.SaveChangesAsync();

                await _userManager.DeleteAsync(user);
            }

            var admin = await _userManager.FindByEmailAsync(User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);
            var UserRole = await _userManager.GetRolesAsync(admin);
            ViewBag.Role = UserRole.FirstOrDefault();

            return RedirectToAction(nameof(Index));
        }

    }

}
