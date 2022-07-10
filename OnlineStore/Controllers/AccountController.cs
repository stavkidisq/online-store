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
        private readonly SignInManager<AppUserModel> _signInManager;

        public AccountController(UserManager<AppUserModel> userManager, SignInManager<AppUserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

        //GET: /account/login
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            UserAuthenticateModel urlTo = new UserAuthenticateModel()
            {
                ReturnUrl = returnUrl
            };

            return View(urlTo);
        }

        //POST: /account/login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserAuthenticateModel userModel)
        {
            if (ModelState.IsValid)
            {
                AppUserModel appUser = await _userManager.FindByEmailAsync(userModel.Email);

                if(appUser != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = 
                        await _signInManager.PasswordSignInAsync(appUser, userModel.Password, false, false);

                    if(result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(userModel.ReturnUrl) && Url.IsLocalUrl(userModel.ReturnUrl))
                        {
                            return Redirect(userModel.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Products");
                        }
                    }

                    ModelState.AddModelError(string.Empty, "Login failed, wrong credentials!");
                }
            }

            return View(userModel);
        }

        //GET: /account/logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Redirect("/");
        }

        //GET: /account/edit
        public async Task<IActionResult> Edit()
        {
            AppUserModel appUserModel = await _userManager.FindByNameAsync(User?.Identity?.Name);
            UserAuthorizeModel userAuthorizeModel = new UserAuthorizeModel(appUserModel);

            return View(userAuthorizeModel);
        }
    } 
}
