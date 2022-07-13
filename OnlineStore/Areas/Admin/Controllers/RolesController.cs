using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUserModel> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUserModel> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        //GET: /admin/roles
        public IActionResult Index()
        {
            return View(_roleManager.Roles);
        }

        //GET: /admin/roles/create
        public IActionResult Create()
        {
            return View();
        }

        //POST: /admin/roles/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Required, MinLength(4)] string name)
        {
            if(ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(name));

                if(result.Succeeded)
                {
                    TempData["Success"] = "The role has been created!";

                    return RedirectToAction("Index");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Minimal length is 4!");
            }

            return View();
        }

        //GET: /admin/roles/edit
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            List<AppUserModel> members = new List<AppUserModel>();
            List<AppUserModel> nonMembers = new List<AppUserModel>();

            foreach(var user in _userManager.Users)
            {
                var usersList = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                usersList.Add(user);
            }

            return View(new RoleEditModel() { Role = role, Members = members, NonMembers = nonMembers });
        }

        //POST: /admin/roles/edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleEditModel roleEditModel)
        {
            IdentityResult result;

            foreach(var userId in roleEditModel.AddIds ?? new string[] { })
            {
                AppUserModel userModel = await _userManager.FindByIdAsync(userId);
                result = await _userManager.AddToRoleAsync(userModel, roleEditModel.RoleName);
            }

            foreach (var userId in roleEditModel.DeleteIds ?? new string[] { })
            {
                AppUserModel userModel = await _userManager.FindByIdAsync(userId);
                result = await _userManager.RemoveFromRoleAsync(userModel, roleEditModel.RoleName);
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
