using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace Salky.App.Services
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
        private const long BUFFER_SIZE = 1_048_576;

        //  Call this function to remove the key from memory after use for security
        //[DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        //public static extern bool ZeroMemory(IntPtr Destination, int Length);

        /// <summary>
        /// Creates a random salt that will be used to encrypt your file. This method is required on FileEncrypt.
        /// </summary>
        /// <returns></returns>
        public byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[SALT_SIZE];
            using (var rng = RandomNumberGenerator.Create())
                for (int i = 0; i < FILL_SALT_N_TIMES; i++)
                    rng.GetBytes(data);
            return data;
        }


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
        public string Decrypt(byte[] password, byte[] data)
        {
            var salt = new byte[SALT_SIZE];
            for (int i = 0; i < SALT_SIZE; i++) salt[i] = data[i];
            data = data.Skip(SALT_SIZE).ToArray();

            var aes = CreateAES(password, salt);
            var plainTextData = aes.DecryptCfb(data, aes.IV, AES_PADDING_MODE);
            var plainText = Encoding.UTF8.GetString(plainTextData);
            return plainText;
        }

        private static Aes CreateAES(byte[] passwordBytes, byte[] salt)
        {
            //Set Rijndael symmetric encryption algorithm
            Aes AES = Aes.Create();
            AES.KeySize = AES_KEY_SIZE;
            AES.BlockSize = AES_BLOCK_SIZE;
            AES.Padding = AES_PADDING_MODE;
            //http://stackoverflow.com/questions/2659214/why-do-i-need-to-use-the-rfc2898derivebytes-class-in-net-instead-of-directly
            //"What it does is repeatedly hash the user password along with the salt." High iteration counts.
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, RFC_2898_DERIVE_BYTES_ITERRATION_COUNT);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
            AES.Mode = AES_CIPER_MODE;
            return AES;
        }



        ///// <summary>
        ///// Encrypts a file from its path and a plain password.
        ///// </summary>
        ///// <param name="inputFile"></param>
        ///// <param name="password"></param>
        //public void FileEncrypt(string inputFile, string password)
        //{
        //    //http://stackoverflow.com/questions/27645527/aes-encryption-on-large-files

        //    //create output file name
        //    FileStream fsCrypt = new FileStream(inputFile + ".aes", FileMode.Create);

        //    //generate random salt
        //    byte[] salt = GenerateRandomSalt();
        //    Aes AES = CreateAES(password, salt);
        //    // write salt to the begining of the output file, so in this case can be random every time
        //    fsCrypt.Write(salt, 0, salt.Length);
        //    CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write);
        //    FileStream fsIn = new FileStream(inputFile, FileMode.Open);
        //    //create a buffer (1mb) so only this amount will allocate in the memory and not the whole file
        //    byte[] buffer = new byte[BUFFER_SIZE];
        //    int read;

        //    try
        //    {
        //        while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
        //        {
        //            //Application.DoEvents(); // -> for responsive GUI, using Task will be better!
        //            cs.Write(buffer, 0, read);
        //        }

        //        // Close up
        //        fsIn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error: " + ex.Message);
        //    }
        //    finally
        //    {
        //        cs.Close();
        //        fsCrypt.Close();
        //    }
        //}
        ///// <summary>
        ///// Decrypts an encrypted file with the FileEncrypt method through its path and the plain password.
        ///// </summary>
        ///// <param name="inputFile"></param>
        ///// <param name="outputFile"></param>
        ///// <param name="password"></param>
        //public static void FileDecrypt(string inputFile, string outputFile, string password)
        //{
        //    byte[] salt = new byte[SALT_SIZE];
        //    Aes AES =CreateAES(password, salt);
        //    FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
        //    fsCrypt.Read(salt, 0, salt.Length);
        //    CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);
        //    FileStream fsOut = new FileStream(outputFile, FileMode.Create);
        //    int read;
        //    byte[] buffer = new byte[BUFFER_SIZE];
        //    try
        //    {
        //        while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
        //        {
        //            //Application.DoEvents();
        //            fsOut.Write(buffer, 0, read);
        //        }
        //    }
        //    catch (CryptographicException ex_CryptographicException)
        //    {
        //        Console.WriteLine("CryptographicException error: " + ex_CryptographicException.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error: " + ex.Message);
        //    }

        //    try
        //    {
        //        cs.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error by closing CryptoStream: " + ex.Message);
        //    }
        //    finally
        //    {
        //        fsOut.Close();
        //        fsCrypt.Close();
        //    }
        //}
    }
}
