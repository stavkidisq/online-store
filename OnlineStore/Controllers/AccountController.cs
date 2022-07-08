using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //GET: /account/register
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
    }
}
