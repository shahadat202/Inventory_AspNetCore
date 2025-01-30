using Inventory.Infrastructure.Identity;
using Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Inventory.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MemberController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public MemberController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin,Support")]
        public async Task<IActionResult> AllRoles()
        {
            var roles = await _roleManager.Roles
                .Select(x => new RoleViewModel { Id = x.Id, Name = x.Name})
                .ToListAsync();

            return View(roles);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateRole()
        {
            var model = new RoleCreateModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole(RoleCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new ApplicationRole
                {
                    Id = Guid.NewGuid(),
                    NormalizedName = model.Name.ToUpper(),
                    Name = model.Name,
                    ConcurrencyStamp = DateTime.UtcNow.Ticks.ToString()
                });
                if (result.Succeeded)
                {
                    return RedirectToAction("AllRoles");
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ChangeRole()
        {
            var model = new RoleChangeModel();
            LoadValues(model);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRole(RoleChangeModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);
                var newRole = await _roleManager.FindByIdAsync(model.RoleId.ToString());
                await _userManager.AddToRoleAsync(user, newRole.Name);
            }
            LoadValues(model);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("AllRoles");
        }

        [Authorize(Roles = "Admin,Support")]
        public async Task<IActionResult> UserRoles()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Role = roles.Any() ? string.Join(", ", roles) : "No Role Assigned",
                });
            }
            return View(userRoles);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("UserRoles");
        }

        private void LoadValues(RoleChangeModel model)
        {
            var users = from c in _userManager.Users.ToList() select c;
            var roles = from c in _roleManager.Roles.ToList() select c;

            model.UserId = users.First().Id;
            model.RoleId = roles.First().Id;

            model.Users = new SelectList(users, "Id", "UserName");
            model.Roles = new SelectList(roles, "Id", "Name");
        }
        
    }
}
