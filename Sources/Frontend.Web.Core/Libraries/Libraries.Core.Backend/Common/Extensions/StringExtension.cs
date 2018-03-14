using System;

namespace Libraries.Core.Backend.Common.Extensions
{
    public static class StringExtension
    {
        public static bool CompareIgnoreCase(this string sender, string value)
        {
            return string.Compare(sender, value, StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}
