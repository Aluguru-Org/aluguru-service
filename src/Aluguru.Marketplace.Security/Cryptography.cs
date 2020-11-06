using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Aluguru.Marketplace.Security
{
    public interface ICryptography
    {
        string Encrypt(string text);
        string Decrypt(string encryptedText);
        string CreateRandomHash();
    }

    public class Cryptography : ICryptography
    {
        private readonly SecuritySettings _settings;

        public Cryptography(IOptions<SecuritySettings> options)
        {
            _settings = options.Value;
        }

        public string Encrypt(string text)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(text)));
        }

        public string Decrypt(string text)
        {
            return Convert.ToBase64String(Decrypt(Encoding.UTF8.GetBytes(text)));
        }

        private byte[] Encrypt(byte[] data)
        {
            var pdb = new Rfc2898DeriveBytes(_settings.SecretKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

            using (Aes aes = new AesManaged())
            {
                aes.Key = pdb.GetBytes(aes.KeySize / 8);
                aes.IV = pdb.GetBytes(aes.BlockSize / 8);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.Close();
                    }
                    return memoryStream.ToArray();
                }
            }
        }

        private byte[] Decrypt(byte[] data)
        {
            var pdb = new Rfc2898DeriveBytes(_settings.SecretKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

            using (Aes aes = new AesManaged())
            {
                aes.Key = pdb.GetBytes(aes.KeySize / 32);
                aes.IV = pdb.GetBytes(aes.BlockSize / 16);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.Close();
                    }
                    return memoryStream.ToArray();
                }
            }
        }

        public string CreateRandomHash()
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
