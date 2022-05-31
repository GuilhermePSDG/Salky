using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Salky.App.Security
{
    public partial class RsaService : IDisposable
    {
        private readonly RSAEncryptionPadding CryptoPadding = RSAEncryptionPadding.OaepSHA512;

        public string Id = Guid.NewGuid().ToString();
        private RSA cryptoService;
        private RsaService(RSA cryptoService) => this.cryptoService = cryptoService;
        public string GetPublicKey() => Convert.ToBase64String(cryptoService.ExportRSAPublicKey());
        public string GetPrivateKey() => Convert.ToBase64String(cryptoService.ExportRSAPrivateKey());
        public static RsaService FromPrivateKey(string privateKey)
        {
            var csp = RSA.Create();
            var privateKeyArr = Convert.FromBase64String(privateKey);
            csp.ImportPkcs8PrivateKey(privateKeyArr, out int total);
            return new RsaService(csp);
        }
        public static RsaService FromPublicKey(string publickey)
        {
            var csp = RSA.Create();
            var publickeyKeyArr = Convert.FromBase64String(publickey);
            csp.ImportSubjectPublicKeyInfo(publickeyKeyArr, out int total);
            return new RsaService(csp);
        }
        public static RsaService GenereteNewKeys(int size = 10240)
        {
            var cryptoService = new RSACryptoServiceProvider(size);
            return new RsaService(cryptoService);
        }

        public string Decrypt(string encryptedtext)
        {
            var encryptedData = Convert.FromBase64String(encryptedtext);
            var decryptedData = cryptoService.Decrypt(encryptedData, CryptoPadding);
            var plainTextData = Encoding.UTF8.GetString(decryptedData);
            return plainTextData;
        }
        public string Encrypt(string plainText)
        {
            var plainTextData = Encoding.UTF8.GetBytes(plainText);
            var textEncryptedData = cryptoService.Encrypt(plainTextData, CryptoPadding);
            var textEncryptedStr = Convert.ToBase64String(textEncryptedData);
            return textEncryptedStr;
        }


        public void Dispose()
        {
            cryptoService.Dispose();
            cryptoService = null;
        }
    }
}
