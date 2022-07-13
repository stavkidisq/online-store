using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Models
{
    public class RoleEditModel
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<AppUserModel> Members { get; set; }
        public IEnumerable<AppUserModel> NonMembers { get; set; }
        public string RoleName { get; set; }
        public string[] AddIds { get; set; }
        public string[] DeleteIds { get; set; }
    }
}
