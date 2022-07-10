using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class UserEditModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [DataType(DataType.Password), MinLength(6, ErrorMessage = "Minimum length is 6")]
        public string Password { get; set; } = null!;

        public UserEditModel() { }

        public UserEditModel(AppUserModel appUserModel)
        {
            Email = appUserModel.Email;
            Password = appUserModel.PasswordHash;
        }
    }
}
