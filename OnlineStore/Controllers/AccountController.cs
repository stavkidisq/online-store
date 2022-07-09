using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;

        public AccountController(UserManager<AppUserModel> userManager)
        {
            _userManager = userManager;
        }

        //GET: /account/register
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        //POST: /account/register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserAuthorizeModel userModel)
        {
            if(ModelState.IsValid)
            {
                AppUserModel appUserModel = new AppUserModel()
                {
                    UserName = userModel.UserName,
                    Email = userModel.Email
                };

                IdentityResult result = await _userManager.CreateAsync(appUserModel, userModel.Password);

                if(result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(userModel);
        }
    }
}
