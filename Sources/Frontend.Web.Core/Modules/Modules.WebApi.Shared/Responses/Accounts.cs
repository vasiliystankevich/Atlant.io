using System;
using System.Collections.Generic;
using System.Web;

namespace Modules.WebApi.Shared.Responses
{
    public class LoginAccountResponse : OkResponse
    {
        public LoginAccountResponse() { }
        public LoginAccountResponse(Guid userId, HttpCookie cookie)
        {
            UserId = userId;
            Coockie = new KeyValuePair<string, string>(cookie.Name, cookie.Value);
        }
        public Guid UserId { get; set; }
        public KeyValuePair<string, string> Coockie { get; set; }
    }
}
