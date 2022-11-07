using ManageUserIdentity.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManageUserIdentity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var model = _userManager.Users.Select(user => new UsersViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                UserRoles = _userManager.GetRolesAsync(user).Result.ToList()
            }).ToList();

            return View(model);
        }

        public async Task<IActionResult> Add(string id)
        {
            var roles = await _roleManager.Roles.ToListAsync();

            if (!string.IsNullOrEmpty(id))
            {
                var userToEdit = await _userManager.FindByIdAsync(id);
                if (userToEdit is null) return NotFound();

                var editModel = new AddUserViewModel
                {
                    id = id,
                    Email = userToEdit.Email,
                    UserName = userToEdit.UserName,
                    FirstName = userToEdit.FirstName,
                    Roles = roles.Select(r => new RoleModel
                    {
                        RoleId = r.Id,
                        RoleName = r.Name,
                        IsRoleSelected = _userManager.IsInRoleAsync(userToEdit, r.Name).Result
                    }).ToList()
                };

                ViewData["IsEdit"] = true;
                return View(editModel);
            }

            var model = new AddUserViewModel
            {
                Roles = roles.Select(r => new RoleModel { RoleName = r.Name, RoleId = r.Id }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUserViewModel model, string id)
        {
          
            if (!string.IsNullOrEmpty(id))
            {
                // then this is an edit action
                var userToEdit = await _userManager.FindByIdAsync(id);
                if (userToEdit is null) return NotFound();

                var userEmailExist = await _userManager.FindByEmailAsync(model.Email);
                if (model.Email != userToEdit.Email && userEmailExist is not null)
                {
                    ModelState.AddModelError("Email", "This email is registered");
                    return View(model);
                }

                var thisUserNameExist = await _userManager.FindByNameAsync(model.UserName);
                if (model.UserName != userToEdit.UserName && thisUserNameExist is not null)
                {
                    ModelState.AddModelError("UserName", "This Username is registered");
                    return View(model);
                }

                userToEdit.UserName = model.UserName;
                userToEdit.Email = model.Email;
                userToEdit.FirstName = model.FirstName;

                await _userManager.UpdateAsync(userToEdit);
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                if (!model.Roles.Any(r => r.IsRoleSelected))
                {
                    ModelState.AddModelError("Roles", "You must select minimum one role");
                    return View(model);
                }

                var emailExist = await _userManager.FindByEmailAsync(model.Email);
                if (emailExist is not null)
                {
                    ModelState.AddModelError("Email", "This email is registered");
                    return View(model);
                }

                var userNameExist = await _userManager.FindByNameAsync(model.UserName);
                if (userNameExist is not null)
                {
                    ModelState.AddModelError("UserName", "This Username is registered");
                    return View(model);
                }

                var user = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(user, model.Roles.Where(r => r.IsRoleSelected).Select(rl => rl.RoleName).ToList());
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("Roles", result.Errors.Select(e => e.Description).First() );
            }


            return View(model);
        }

        public async Task<IActionResult> EditUserRoles(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user is null)
            {
                return NotFound();
            }

            var allRoles = await _roleManager.Roles.ToListAsync();

            var model = new UserRolesViewModel
            {
                Roles = allRoles.Select(r => new RoleModel
                {
                    RoleId = r.Id,
                    RoleName = r.Name,
                    IsRoleSelected = _userManager.IsInRoleAsync(user, r.Name).Result
                }).ToList(),
                UserId = user.Id,
                UserName = user.UserName
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserRoles(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user is null)
            {
                return NotFound();
            }

            var allUserRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in model.Roles)
            {
                if (allUserRoles.Any(r => r == role.RoleName) && !role.IsRoleSelected)
                {
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                }

                if (!allUserRoles.Any(r => r == role.RoleName) && role.IsRoleSelected)
                {
                    await _userManager.AddToRoleAsync(user, role.RoleName);
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
