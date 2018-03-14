namespace Modules.Core.Accounts
{
    public class LoginModel
    {
        public LoginModel()
        {
            IsVisibleRecaptcha = false;
        }

        public string Account { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public bool IsVisibleRecaptcha { get; set; }
    }
}
