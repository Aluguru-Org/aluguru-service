using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Aluguru.Marketplace.Security
{
    public static class Cryptography
    {
        public static string Encrypt(string text)
        {
            return Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(text)));
        }

        public static string CreateRandomHash()
        {
            var rand = new Random().Next(0, 1000).ToString();
            var hashData = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(rand));

            var sBuilder = new StringBuilder();

            for (int i = 0; i < hashData.Length; i ++)
            {
                sBuilder.Append(hashData[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }        
    }
}
