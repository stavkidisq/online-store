using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class UserAuthenticateModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required, DataType(DataType.Password), MinLength(6, ErrorMessage = "Minimum length is 6")]
        public string Password { get; set; } = null!;

        public string? ReturnUrl { get; set; }
    }
}
