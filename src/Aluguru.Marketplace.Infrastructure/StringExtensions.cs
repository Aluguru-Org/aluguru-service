using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Infrastructure
{
    public static class StringExtensions
    {
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
