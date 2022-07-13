﻿using Microsoft.AspNetCore.Identity;
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
    }
}
