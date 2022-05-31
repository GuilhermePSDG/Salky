using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace Salky.App.Security
{
    public class AESservice
    {
        private const int SALT_SIZE = 32;
        private const int FILL_SALT_N_TIMES = 10;
        private const int AES_KEY_SIZE = 256;
        private const int AES_BLOCK_SIZE = 128;
        private const PaddingMode AES_PADDING_MODE = PaddingMode.PKCS7;
        private const CipherMode AES_CIPER_MODE = CipherMode.CFB;
        private const int RFC_2898_DERIVE_BYTES_ITERRATION_COUNT = 50_000;

        public byte[] Encrypt(byte[] password, byte[] plaintextdata)
        {
            var salt = GenerateRandomSalt();
            var aes = CreateAES(password, salt);
            var encryptedData = aes.EncryptCfb(plaintextdata, aes.IV, AES_PADDING_MODE);
            byte[] SaltAndData = new byte[salt.Length + encryptedData.Length];
            salt.CopyTo(SaltAndData, 0);
            encryptedData.CopyTo(SaltAndData, salt.Length);
            return SaltAndData;
        }
        public byte[] Decrypt(byte[] password, byte[] data)
        {
            var salt = new byte[SALT_SIZE];
            for (int i = 0; i < SALT_SIZE; i++) salt[i] = data[i];
            data = data.Skip(SALT_SIZE).ToArray();
            var aes = CreateAES(password, salt);
            var plainTextData = aes.DecryptCfb(data, aes.IV, AES_PADDING_MODE);
            return plainTextData;
        }

        private static Aes CreateAES(byte[] passwordBytes, byte[] salt)
        {
            Aes AES = Aes.Create();
            AES.KeySize = AES_KEY_SIZE;
            AES.BlockSize = AES_BLOCK_SIZE;
            AES.Padding = AES_PADDING_MODE;
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, RFC_2898_DERIVE_BYTES_ITERRATION_COUNT, HashAlgorithmName.SHA512);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Mode = AES_CIPER_MODE;
            return AES;
        }
        private byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[SALT_SIZE];
            using (var rng = RandomNumberGenerator.Create())
                for (int i = 0; i < FILL_SALT_N_TIMES; i++)
                    rng.GetBytes(data);
            return data;
        }
    }
}