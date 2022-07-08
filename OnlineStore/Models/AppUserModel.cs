using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Models
{
    public class AppUserModel : IdentityUser
    {
        public string Occupation { get; set; }
    }
}
