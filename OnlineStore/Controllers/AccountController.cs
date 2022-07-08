using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public string Index()
        {
            return "Hello!";
        }
    }
}
