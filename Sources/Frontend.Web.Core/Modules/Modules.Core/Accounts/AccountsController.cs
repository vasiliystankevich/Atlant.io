using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Libraries.Core.Backend.Authorization;
using Libraries.Core.Backend.Common;
using Libraries.Core.Backend.Recaptcha;
using Translations;
using WebMatrix.WebData;

namespace Modules.Core.Accounts
{
    [Authorize(Roles = ERoles.Administrator)]
    [InitializeSimpleMembership]
    public class AccountsController : BaseController<IRecaptchaRepository>
    {
        public AccountsController(IRecaptchaRepository recaptchaRepository):base(recaptchaRepository)
        {
        }

        [AllowAnonymous]
        [InitializeSimpleMembership]
        public async Task<ActionResult> Login(string returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;
            return await GeneratorActionResult("~/Accounts/Login/action.cshtml", new LoginModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [InitializeSimpleMembership]
        public async Task<ActionResult> Login(LoginModel model)
        {
            ActionResult result;
            try
            {
                if (!ValidationRecaptcha())
                {
                    ModelState.AddModelError("", I18NEngine.GetString("modules.core.accounts", "controller_login_error_captcha"));
                    CheckVisibleRecaptcha();
                    result = CheckVisibleRecaptcha();
                    return await GetAsyncResult(result);
                }
                if (WebSecurity.Login(model.Account, model.Password, model.RememberMe))
                {
                    ClearRecaptcha();
                    var authorizationResult = GenerateAuthorizationResult();
                    return await GetAsyncResult(authorizationResult);
                }
                CheckExistLoginError();
                ModelState.AddModelError("", I18NEngine.GetString("modules.core.accounts", "controller_login_error_verify"));
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("", exception.Message);
            }
            result = CheckVisibleRecaptcha();
            return await GetAsyncResult(result);
        }

        protected ActionResult GenerateAuthorizationResult()
        {
            ActionResult result;
            if (TempData["ReturnUrl"] != null)
            {
                if ((string) TempData["ReturnUrl"] == "/")
                {
                    result = RedirectToAction("Index", "Home");
                }
                else result = Redirect((string) TempData["ReturnUrl"]);
            }
            else result = RedirectToAction("Index", "Home");
            return result;
        }

        private void ClearRecaptcha()
        {
            Session["checkRecaptcha"] = null;
            Session["loginErrorCount"] = null;
        }

        private bool ValidationRecaptcha()
        {
            if (Session["checkRecaptcha"] == null) return true;
            if (string.IsNullOrWhiteSpace(Request.Form["g-Recaptcha-Response"])) return false;
            if (!RecaptchaRepository.Validate(Request.Form["g-Recaptcha-Response"])) return false;
            ClearRecaptcha();
            return true;
        }

        private ActionResult CheckVisibleRecaptcha()
        {
            var countLoginErrorForVisibleRecaptcha =
                Convert.ToInt32(ConfigurationManager.AppSettings["countLoginErrorForVisibleRecaptcha"]);
            var verivyValue = Session["loginErrorCount"] == null ||
                              Convert.ToInt32(Session["loginErrorCount"]) < countLoginErrorForVisibleRecaptcha;
            if (verivyValue) return View("~/Accounts/Login/action.cshtml", new LoginModel());
            Session["checkRecaptcha"] = true;
            return View("~/Accounts/Login/action.cshtml", new LoginModel { IsVisibleRecaptcha = true });
        }

        private void CheckExistLoginError()
        {
            if (Session["loginErrorCount"] == null)
                Session["loginErrorCount"] = 1;
            else Session["loginErrorCount"] = Convert.ToInt32(Session["loginErrorCount"]) + 1;
        }

        [AllowAnonymous]
        [InitializeSimpleMembership]
        public async Task<RedirectToRouteResult> Logout()
        {
            WebSecurity.Logout();
            var action = RedirectToAction("Index", "Home");
            return await GetAsyncResult(action);
        }

        IRecaptchaRepository RecaptchaRepository { get; }
    }
}
