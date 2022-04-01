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



        private RSACryptoServiceProvider cryptoService;
        public string Id = Guid.NewGuid().ToString();
        public string GetPublicKey()
        {
            var pvks = cryptoService.ExportRSAPublicKey();
            var pvksStr = Convert.ToBase64String(pvks);
            return pvksStr;
        }
        public string GetPrivateKey()
        {
            var pvks = cryptoService.ExportRSAPrivateKey();
            var pvksStr = Convert.ToBase64String(pvks);
            return pvksStr;
        }

        private RsaService(RSACryptoServiceProvider cryptoServiceProvider)
        {
            cryptoService = cryptoServiceProvider;
        }

        public static RsaService GenereteNewKeys(int size = 10240)
        {
            var cryptoService = new RSACryptoServiceProvider(size);
            return new RsaService(cryptoService);
        }
        public static RsaService FromPublicKey(string pubKeyString)
        {
            var csp = new RSACryptoServiceProvider();
            csp.ImportRSAPublicKey(Convert.FromBase64String(pubKeyString), out int total);
            return new RsaService(csp);
        }
        public static RsaService FromPrivateKey(string privateKey)
        {
            var csp = new RSACryptoServiceProvider();
            csp.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out int total);
            return new RsaService(csp);
        }

        public string Decrypt(string data)
        {
            var bytesCypherText = Convert.FromBase64String(data);
            var bytesPlainTextData = cryptoService.Decrypt(bytesCypherText, false);
            var plainTextData = Encoding.Unicode.GetString(bytesPlainTextData);
            return plainTextData;
        }

        public string Encrypt(string text)
        {
            var bytesPlainTextData = Encoding.Unicode.GetBytes(text);
            var bytesCypherText = cryptoService.Encrypt(bytesPlainTextData, false);
            return Convert.ToBase64String(bytesCypherText);
        }

        public void Dispose()
        {
            cryptoService.Dispose();
            cryptoService = null;
        }
    }
}
