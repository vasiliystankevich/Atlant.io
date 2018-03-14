using System;
using System.Web.Mvc;

namespace Translations
{
    public static class I18NEngine
    {
        private static string GetStringForId(string key, string id)
        {
            var result = string.Empty;
            switch (key)
            {
                case "common": result = i18n.common.messages.ResourceManager.GetString(id); break;

                case "modules.core.accounts.controller": result = i18n.modules.core.accounts.controller.messages.ResourceManager.GetString(id); break;
                case "modules.core.accounts.login": result = i18n.modules.core.accounts.login.messages.ResourceManager.GetString(id); break;

                case "modules.site.home.controller": result = i18n.modules.site.home.controller.messages.ResourceManager.GetString(id); break;
                case "modules.site.home.index": result = i18n.modules.site.home.index.messages.ResourceManager.GetString(id); break;
            }
            return result;
        }

        public static string GetString(string key, string id)
        {
            var result = GetStringForId(key, id);
            if (string.IsNullOrWhiteSpace(result))
                result = GetStringForId("common", "resource_not_found");
            return result;
        }

        public static MvcHtmlString GetMvcHtmlString(string key, string id)
        {
            var text = GetString(key, id);
            return MvcHtmlString.Create(text);
        }

        public static string GetLocaleString<TEnum>(this TEnum sender, string key)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var nameType = typeof(TEnum).Name.ToLowerInvariant();
            var nameValue = Enum.GetName(typeof(TEnum), sender).ToLowerInvariant();
            return GetString(key, $"{nameType}_{nameValue}");
        }

        public static T ParseEnum<T>(this string value) where T : struct
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string GetLocaleString(this object sender, string key, string id) { return GetString(key, id); }

        public static string GetLocaleErrorString(string id) { return GetString("error.controller", id); }
    }
}
