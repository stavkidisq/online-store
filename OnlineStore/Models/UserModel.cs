namespace OnlineStore.Models
{
    public class UserModel
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        UserModel() { }

        public UserModel(AppUserModel appUserModel) 
        {
            UserName = appUserModel.UserName;
            Email = appUserModel.Email;
            Password = appUserModel.PasswordHash;
        }
    }
}
