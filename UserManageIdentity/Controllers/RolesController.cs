using ManageUserIdentity.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ManageUserIdentity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            List<IdentityRole> roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public async Task<IActionResult> Add(RoleViewModel model)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index), _roleManager.Roles.ToList());

            if (await _roleManager.RoleExistsAsync(model.RoleName))
            {
                ModelState.AddModelError("RoleName", "Role Already Exist");
                return View(nameof(Index), _roleManager.Roles.ToList());
            }

            var res = await _roleManager.CreateAsync(new IdentityRole(model.RoleName));

            return RedirectToAction(nameof(Index));
        }
    }
}
