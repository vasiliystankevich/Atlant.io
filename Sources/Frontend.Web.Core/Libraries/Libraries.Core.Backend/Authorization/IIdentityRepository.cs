using Dal;

namespace Libraries.Core.Backend.Authorization
{
    public interface IIdentityRepository
    {

        void Initialize();
        void CreateDefaultRoles();
        void CreateUser(AccountModel model, string password);
        void CreateSystemAccount();
        void CreateDefaultAdministrator();
        void CreateDefaultUser();
        IDalContext Context { get; set; }
    }
}
