using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Shared
{
    public partial class RsaService : IDisposable
    {
        public string Id = Guid.NewGuid().ToString();
        private RSACryptoServiceProvider cryptoService;
        private RsaService(RSACryptoServiceProvider cryptoService) => this.cryptoService = cryptoService;
        public byte[] GetPublicKey() => cryptoService.ExportRSAPublicKey();
        public byte[] GetPrivateKey() => cryptoService.ExportRSAPrivateKey();
        public static RsaService FromPrivateKey(byte[] privateKey)
        {
            var csp = new RSACryptoServiceProvider();
            csp.ImportRSAPrivateKey(privateKey, out int total);
            return new RsaService(csp);
        }
        public static RsaService FromPublicKey(byte[] publickey)
        {
            var csp = new RSACryptoServiceProvider();
            csp.ImportRSAPublicKey(publickey, out int total);
            return new RsaService(csp);
        }
        public static RsaService GenereteNewKeys(int size = 10240)
        {
            var cryptoService = new RSACryptoServiceProvider(size);
            return new RsaService(cryptoService);
        }
        /// <summary>
        /// USE UTF8
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <returns></returns>
        public string Decrypt(byte[] encryptedData)
        {
            var decryptedData = cryptoService.Decrypt(encryptedData, false);
            var plainTextData = Encoding.UTF8.GetString(decryptedData);
            return plainTextData;
        }
        public byte[] DecryptToBytes(byte[] encryptedData)
        {
            return cryptoService.Decrypt(encryptedData, false);
        }
        /// <summary>
        /// USE UTF8
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public byte[] Encrypt(string plainText)
        {
            var plainTextData = Encoding.UTF8.GetBytes(plainText);
            var textEncryptedData = cryptoService.Encrypt(plainTextData, false);
            return textEncryptedData;
        }
        public byte[] Encrypt(byte[] plainTextData)
        {
            var textEncryptedData = cryptoService.Encrypt(plainTextData, false);
            return textEncryptedData;
        }
        public void Dispose()
        {
            cryptoService.Dispose();
            cryptoService = null;
        }
    }
}
