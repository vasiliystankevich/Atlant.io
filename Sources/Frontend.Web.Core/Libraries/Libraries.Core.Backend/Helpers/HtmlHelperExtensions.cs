using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Libraries.Core.Backend.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString JsScriptLink(this HtmlHelper helper, Uri jsLink)
        {
            return helper.Partial("~/Helpers/JsScriptLink.cshtml", jsLink);
        }

        public static MvcHtmlString JsScriptLink<T>(this HtmlHelper<T> helper, Uri jsLink)
        {
            return helper.Partial("~/Helpers/JsScriptLink.cshtml", jsLink);
        }
    }
}
