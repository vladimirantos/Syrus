using System;
using System.Collections.Generic;
using System.Text;

namespace Syrus.Shared
{
    public static class StringExtensions
    {
        public static string FirstToUpper(this string str)
        {
            if (string.IsNullOrEmpty(str))
                throw new ArgumentException("String cannot be null");
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }
    }
}
