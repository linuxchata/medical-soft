using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Logger;

namespace Utilities
{
    /// <summary>
    /// Represents protection logic.
    /// </summary>
    public class Protection : IDisposable
    {
        private readonly SymmetricAlgorithm cryptoService = new DESCryptoServiceProvider();

        private bool disposed;

        private string tempIv = "7b46123aaed34e869af8";

        /// <summary>
        /// Encrypt specified string.
        /// </summary>
        /// <param name="sourceText">Text to encrypt.</param>
        /// <param name="key">The key.</param>
        /// <returns>Returns encrypted string.</returns>
        public string Encrypt(string sourceText, string key)
        {
            try
            {
                // Create a memory stream.
                var objMemStream = new MemoryStream();

                // Set the legal keys and initialization vectors.
                this.cryptoService.Key = this.GetLegalSecretKey(key);
                this.cryptoService.IV = this.GetLegalIv();

                // Create a CryptoStream using the memory stream and the cryptographic service provider version.
                // of the Data Encryption standard algorithm key.
                var objCryptStream = new CryptoStream(objMemStream, this.cryptoService.CreateEncryptor(), CryptoStreamMode.Write);

                // Create a StreamWriter to write a string to the stream.
                var objStreamWriter = new StreamWriter(objCryptStream);

                // Write the sourceText to the memory stream.
                objStreamWriter.WriteLine(sourceText);

                // Close the StreamWriter and CryptoStream objects.
                objStreamWriter.Close();
                objCryptStream.Close();

                // Get an array of bytes that represents the memory stream.
                var outputBuffer = objMemStream.ToArray();

                // Close the memory stream.
                objMemStream.Close();

                // Return the encrypted byte array.
                return Convert.ToBase64String(outputBuffer);
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// Decrypt specified string.
        /// </summary>
        /// <param name="encriptedText">Text to decrypt.</param>
        /// <param name="key">The key.</param>
        /// <returns>Returns decrypted string.</returns>
        public string Decrypt(string encriptedText, string key)
        {
            try
            {
                // Convert the text into bytes.
                var ecriptedBytes = Convert.FromBase64String(encriptedText);

                // Create a memory stream to the passed buffer.
                var objMemStream = new MemoryStream(ecriptedBytes);

                // Set the legal keys and initialization vectors.
                this.cryptoService.Key = this.GetLegalSecretKey(key);
                this.cryptoService.IV = this.GetLegalIv();

                // Create a CryptoStream using the memory stream and the cryptographic service provider version
                // of the Data Encryption standard algorithm key.
                var objCryptStream = new CryptoStream(objMemStream, this.cryptoService.CreateDecryptor(), CryptoStreamMode.Read);

                // Create a StreamReader for reading the stream.
                var objstreamReader = new StreamReader(objCryptStream);

                // Read the stream as a string.
                var outputText = objstreamReader.ReadLine();

                // Close the streams.
                objstreamReader.Close();
                objCryptStream.Close();
                objMemStream.Close();

                return outputText;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// Dispose the object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the object.
        /// </summary>
        /// <param name="disposing">Define whether managed objects have to be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.cryptoService.Dispose();
            }

            this.disposed = true;
        }

        private byte[] GetLegalSecretKey(string secretKey)
        {
            var tempKey = secretKey;
            this.cryptoService.GenerateKey();
            var tempBytes = this.cryptoService.Key;

            var secretKeyLength = tempBytes.Length;

            if (tempKey.Length > secretKeyLength)
            {
                tempKey = tempKey.Substring(0, secretKeyLength);
            }
            else if (tempKey.Length < secretKeyLength)
            {
                tempKey = tempKey.PadRight(secretKeyLength, ' ');
            }

            return Encoding.ASCII.GetBytes(tempKey);
        }

        private byte[] GetLegalIv()
        {
            this.cryptoService.GenerateIV();
            var tempBytes = this.cryptoService.IV;
            var len = tempBytes.Length;

            this.tempIv = this.tempIv.Length < len ? this.tempIv.PadRight(len, ' ') : this.tempIv.Substring(0, len);

            return Encoding.ASCII.GetBytes(this.tempIv);
        }
    }
}