using System;
using System.Linq;
using System.Security.Authentication;
using System.Web.Security;
using Dal;
using log4net;
using Libraries.Core.Backend.Authorization;
using Libraries.Core.Backend.Common;
using Modules.WebApi.Shared.Requests;
using Modules.WebApi.Shared.Responses;
using Project.Kernel;
using WebMatrix.WebData;

namespace Modules.WebApi.Accounts
{
    public interface IAccountsApiRepository:IVoidAssert<IAccountsApiRepository, string>
    {
        LoginAccountResponse Login(LoginAccountRequest request);

        BaseResponse Logout(BaseRequest request);
        IIdentityRepository IdentityRepository { get; set; }
        IDalContext Context { get; set; }
    }
    public class AccountsApiRepository : BaseRepository, IAccountsApiRepository
    {
        public AccountsApiRepository(IWrapper<ILog> logger, IIdentityRepository identityRepository, IDalContext context) : base(logger)
        {
            IdentityRepository = identityRepository;
            Context = context;
            if (!WebSecurity.Initialized)
                IdentityRepository.Initialize();
        }

        public LoginAccountResponse Login(LoginAccountRequest request)
        {
            this.Assert(request.UserName);
            if (!WebSecurity.Login(request.UserName, request.Password, request.RememberMe)) throw new AuthenticationException("Bad login or password");
            var cookie = FormsAuthentication.GetAuthCookie(request.UserName, request.RememberMe);
            var userId = Context.Accounts.FirstOrDefault(u => string.Compare(u.AccountName, request.UserName, StringComparison.OrdinalIgnoreCase)==0).UserId.Value;
            return new LoginAccountResponse(userId, cookie);
        }

        public BaseResponse Logout(BaseRequest request)
        {
            WebSecurity.Logout();
            return BaseResponse.Ok();
        }

        void IVoidAssert<IAccountsApiRepository, string>.Assert(string argFirst)
        {
            try
            {
                var user = Context.Accounts.FirstOrDefault(u => string.Compare(argFirst, u.AccountName, StringComparison.OrdinalIgnoreCase) == 0);
                user.IsNull("user not found");
                user.Verify(() => !user.IsActivate.Value, "user not active");
            }
            catch (Exception e)
            {
                throw new AuthenticationException(e.Message);
            }
        }

        public IIdentityRepository IdentityRepository { get; set; }
        public IDalContext Context { get; set; }
    }
}
