using System;
using System.Security.Cryptography;
using System.Text;

namespace Flexylexy.Web.Models
{
    public class Tokenizer : ITokenizer
    {
        private const int MinPaddedReferenceLength = 50;
        private readonly ICryptoTransform _encryptor;
        private readonly ICryptoTransform _decryptor;

        public Tokenizer()
        {
            var aes = new AesManaged
            {
                BlockSize = 128,
                KeySize = 128,
                Mode = CipherMode.CBC,
                Key = Encoding.UTF8.GetBytes("NWL2UmC3gaibcZ4Q"),
                IV = Encoding.UTF8.GetBytes("oJo970IvggCY0OL7")
            };

            _encryptor = aes.CreateEncryptor();
            _decryptor = aes.CreateDecryptor();
        }

        private string Encrypt(string plaintext)
        {
            return Encrypt(Encoding.UTF8.GetBytes(plaintext));
        }

        private string Encrypt(byte[] plaintext)
        {
            var encrypted = _encryptor.TransformFinalBlock(plaintext, 0, plaintext.Length);
            return Convert.ToBase64String(encrypted);
        }

        private string Decrypt(string ciphertext)
        {
            return Decrypt(Convert.FromBase64String(ciphertext));
        }

        private string Decrypt(byte[] ciphertext)
        {
            var decrypted = _decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
            return Encoding.UTF8.GetString(decrypted).Trim();
        }

        private string Pad(string reference)
        {
            int padding = Math.Max(reference.Length, MinPaddedReferenceLength);
            return reference + new String(' ', padding);
        }

        public string CreateToken(string data)
        {
            string paddedData = Pad(data);
            return Encrypt(paddedData);
        }

        public string GetData(string token)
        {
            if (String.IsNullOrWhiteSpace(token)) return null;
            return Decrypt(token);
        }
    }
}