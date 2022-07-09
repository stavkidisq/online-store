using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class UserAuthorizeModel
    {
        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        public string UserName { get; set; } = null!;
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required, DataType(DataType.Password), MinLength(6, ErrorMessage = "Minimum length is 6")]
        public string Password { get; set; } = null!;

        public UserAuthorizeModel() { }

        public UserAuthorizeModel(AppUserModel appUserModel) 
        {
            UserName = appUserModel.UserName;
            Email = appUserModel.Email;
            Password = appUserModel.PasswordHash;
        }
    }
}
