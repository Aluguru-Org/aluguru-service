using System;

namespace Aluguru.Marketplace.Infrastructure
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string src)
        {
            if (string.IsNullOrEmpty(src)) return string.Empty;

            char[] a = src.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
        public static bool IsBase64(this string src)
        {
            if (string.IsNullOrEmpty(src) 
                || src.Length % 4 != 0
                || src.Contains(" ")
                || src.Contains("\t")
                || src.Contains("\r")
                || src.Contains("\n"))
                return false;

            try
            {
                Convert.FromBase64String(src);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
