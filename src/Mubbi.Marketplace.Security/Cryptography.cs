using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Mubbi.Marketplace.Security
{
    public static class Cryptography
    {
        public static string Encrypt(string text)
        {
            return Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(text)));
        }
    }
}
