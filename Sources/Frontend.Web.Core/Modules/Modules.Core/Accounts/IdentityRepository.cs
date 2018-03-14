using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Security;
using Dal;
using log4net;
using Libraries.Core.Backend.Authorization;
using Libraries.Core.Backend.Common;
using Project.Kernel;
using WebMatrix.WebData;

namespace Modules.Core.Accounts
{
    public class IdentityRepository : BaseRepository, IIdentityRepository
    {
        public IdentityRepository(IDalContext context, IWrapper<ILog> logger) : base(logger)
        {
            Context = context;
        }

        public void Initialize()
        {
            try
            {
                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Accounts", "Id", "AccountName",
                    autoCreateTables: true);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CreateDefaultRoles()
        {
            var roleProvider = (SimpleRoleProvider)Roles.Provider;
            var roles = new List<string> { ERoles.System, ERoles.Owner, ERoles.Administrator, ERoles.User };
            roles.ForEach(role => ExistenceRole(roleProvider, role));
        }

        public void CreateUser(AccountModel model, string password)
        {
            CheckAccountName(model.AccountName);
            var confirmToken = WebSecurity.CreateUserAndAccount(model.AccountName, password);
            Roles.AddUserToRole(model.AccountName, model.Role);
            WebSecurity.ConfirmAccount(confirmToken);
            UpdateAccount(model);
        }

        public void CreateDefaultUser(string userName, string role)
        {
            try
            {
                if (WebSecurity.UserExists(ConfigurationManager.AppSettings[$"{userName}UserName"])) return;
                var accountModel = new AccountModel
                {
                    IsActivate = true,
                    AccountName = ConfigurationManager.AppSettings[$"{userName}UserName"],
                    Email = ConfigurationManager.AppSettings[$"{userName}EmailAddress"],
                    Role = role
                };
                CreateUser(accountModel, ConfigurationManager.AppSettings[$"{userName}Password"]);
            }
            catch (MembershipCreateUserException)
            {
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void CreateSystemAccount()
        {
            CreateDefaultUser("SystemAccount", ERoles.System);
        }

        public void CreateDefaultAdministrator()
        {
            CreateDefaultUser("DefaultAdministrator", ERoles.Administrator);
        }

        public void CreateDefaultUser()
        {
            CreateDefaultUser("DefaultUser", ERoles.User);
        }

        protected void ExistenceRole(SimpleRoleProvider provider, string role)
        {
            if (!provider.RoleExists(role))
                provider.CreateRole(role);
        }

        private static void CheckAccountName(string accountName)
        {
            if (WebSecurity.UserExists(accountName)) throw new MembershipCreateUserException("The email and/or username already exist");
        }

        private void UpdateAccount(AccountModel model)
        {
            var account =
                Context.Accounts.First(
                    x => string.Compare(x.AccountName, model.AccountName, StringComparison.OrdinalIgnoreCase) == 0);
            account.UserId = model.UserId;
            account.Email = model.Email;
            account.Description = model.Description;
            account.Role = model.Role;
            account.Note = model.Note;
            account.IsActivate = model.IsActivate;
            Context.SaveChanges();
        }

        public IDalContext Context { get; set; }
    }
}