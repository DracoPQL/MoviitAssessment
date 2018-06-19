using Moviit.Infrastructure.CrossCutting.Utils.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Moviit.Infrastructure.CrossCutting.Utils.Cryptography
{

    /// <summary>
    /// Auxiliary class for encryption and decryption.
    /// </summary>
    public class CryptoUtilities : IDisposable
    {

        #region Constants

        /// <summary>
        /// Symmetric encryption key with wich the values will be encrypted and decrypted.
        /// </summary>
        private byte[] _key = { 200, 151, 181, 90, 97, 252, 62, 121, 241, 216, 181, 179, 228, 235, 88, 49, 71, 167, 145, 53, 133, 78, 102, 172, 44, 115, 238, 169, 94, 252, 37, 86 };

        /// <summary>
        /// Initialization vector for symmetric encryption.
        /// </summary>
        private byte[] _vector = { 199, 6, 201, 63, 8, 121, 120, 161, 84, 36, 54, 139, 198, 227, 45, 33 };

        /// <summary>
        /// Constants for default values
        /// </summary>
        private const int _saltMinLengthDefaultValue = 4;
        private const int _saltMaxLengthDefaultValue = 8;

        #endregion

        #region Variables

        /// <summary>
        /// Transformers to be used in the encryption and decryption.
        /// </summary>
        private ICryptoTransform _encryptor;
        private ICryptoTransform _decryptor;

        /// <summary>
        /// Encoder to use to change arrays of bytes to text and vice versa.
        /// </summary>
        private UTF8Encoding _encoder;

        /// <summary>
        /// Variables to be used in the hashing methods
        /// </summary>
        private int _saltMinLength;
        private int _saltMaxLength;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CryptoUtilities()
        {
            //Use Rijndael as the method for encryption and decryption
            RijndaelManaged method = new RijndaelManaged();

            //Create the encryptor and decryptor with the key, vector and methods
            _encryptor = method.CreateEncryptor(_key, _vector);
            _decryptor = method.CreateDecryptor(_key, _vector);

            //Initialize the encoder
            _encoder = new UTF8Encoding();

            //Get the configuration keys for hashing
            _saltMinLength = Convert.ToInt32(ConfigUtilities.GetValue("HASHING_SALT_MIN_LENGTH", false, _saltMinLengthDefaultValue.ToString()));
            _saltMaxLength = Convert.ToInt32(ConfigUtilities.GetValue("HASHING_SALT_MAX_LENGTH", false, _saltMaxLengthDefaultValue.ToString()));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Encrypts a text and returns it as a string.
        /// </summary>
        /// <param name="text">Text to encrypt</param>
        /// <returns>The encrypted text</returns>
        public string EncryptToString(string text)
        {
            try
            {
                return ByteArrayToString(Encrypt(text));
            }
            catch (Exception ex)
            {
                CryptographicException outerEx = new CryptographicException("The text {0} can't be encrypted", ex);
                outerEx.Data.Add(0, text);
                throw outerEx;
            }
        }

        /// <summary>
        /// Encrypts a text and returns it as an array of bytes.
        /// </summary>
        /// <param name="text">Text to encrypt</param>
        /// <returns>Array of bytes containing the encrypted text</returns>
        public byte[] Encrypt(string text)
        {
            try
            {
                //Convert the text to an array of bytes
                byte[] bytes = _encoder.GetBytes(text);

                //Save the text in the encryptor's stream
                using (MemoryStream stream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(stream, _encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytes, 0, bytes.Length);
                        cryptoStream.FlushFinalBlock();

                        //Get the encrypted text as an array of bytes from the stream
                        stream.Position = 0;
                        byte[] encryptedBytes = new byte[stream.Length];
                        stream.Read(encryptedBytes, 0, encryptedBytes.Length);

                        //Free resources
                        cryptoStream.Close();

                        //Return the result
                        return encryptedBytes;
                    }
                }
            }
            catch (Exception ex)
            {
                CryptographicException outerEx = new CryptographicException("The text {0} can't be encrypted", ex);
                outerEx.Data.Add(0, text);
                throw outerEx;
            }
        }

        /// <summary>
        /// Decrypt a text.
        /// </summary>
        /// <param name="encryptedText">Text to decrypt</param>
        /// <returns>The decrypted text</returns>
        public string DecryptString(string encryptedText)
        {
            try
            {
                return Decrypt(StringToByteArray(encryptedText));
            }
            catch (Exception ex)
            {
                CryptographicException outerEx = new CryptographicException("The text {0} can't be decrypted", ex);
                outerEx.Data.Add(0, encryptedText);
                throw outerEx;
            }
        }

        /// <summary>
        /// Decrypts a byte array and returns it as text.
        /// </summary>
        /// <param name="encryptedArray">Array of bytes to decrypt</param>
        /// <returns>Text containing the array of bytes decrypted</returns>
        public string Decrypt(byte[] encryptedArray)
        {
            try
            {
                //Save the encrypted array of bytes into the stream of the decryptor
                using (MemoryStream stream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(stream, _decryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(encryptedArray, 0, encryptedArray.Length);
                        cryptoStream.FlushFinalBlock();

                        //Get the decrypted text as an array of bytes from the stream
                        stream.Position = 0;
                        byte[] decryptedBytes = new byte[stream.Length];
                        stream.Read(decryptedBytes, 0, decryptedBytes.Length);
                        stream.Close();

                        //Convert the decrypted array of bytes to string and return it
                        return _encoder.GetString(decryptedBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                CryptographicException outerEx = new CryptographicException("The text {0} can't be decrypted", ex);
                outerEx.Data.Add(0, encryptedArray);
                throw outerEx;
            }
        }

        /// <summary>
        /// Hashs a string.
        /// </summary>
        /// <param name="text">String to hash</param>
        /// <returns>The hashed string</returns>
        /// <remarks>This method should be used to encrypt passwords</remarks>
        public string EncryptSHA(string text)
        {
            try
            {
                //Use SHA1 as hasher
                using (SHA1CryptoServiceProvider hasher = new SHA1CryptoServiceProvider())
                {
                    //Get the text to hash as an array of bytes and hash it in another array of bytes
                    byte[] bytes = Encoding.UTF8.GetBytes(text);
                    byte[] hashedArray = hasher.ComputeHash(bytes);
                    hasher.Clear();

                    //Return the hash as a base 64 string
                    return Convert.ToBase64String(hashedArray);
                }
            }
            catch (Exception ex)
            {
                CryptographicException outerEx = new CryptographicException("The text {0} can't be hashed", ex);
                outerEx.Data.Add(0, text);
                throw outerEx;
            }
        }

        /// <summary>
        /// Generates a hash for the given plain text value and returns a base64-encoded encoded result.
        /// The salt is created if is not provided and will be returned as reference so it can be stored later.
        /// </summary>
        /// <param name="plainText">Text to be hashed</param>
        /// <param name="salt">Salt to use in the hashing process. Is created if is not provided and always is returned as reference</param>
        /// <returns>The hashed plain text and salt as a base64-encoded string</returns>
        public string ComputeHash(string plainText, ref string salt)
        {
            //Convert the salt to an array of bytes if it exists or just create it as null if it wasn't
            byte[] saltBytes = (string.IsNullOrWhiteSpace(salt)) ? null : StringToByteArray(salt);

            //If the salt bytes was not specified then create a new one
            if (saltBytes == null)
            {
                saltBytes = CreateSalt(_saltMinLength, _saltMaxLength);
            }
            salt = ByteArrayToString(saltBytes);

            //Allocate array, which will hold plain text and salt and copy both to that array
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];
            Array.Copy(plainTextBytes, 0, plainTextWithSaltBytes, 0, plainTextBytes.Length);
            Array.Copy(saltBytes, 0, plainTextWithSaltBytes, plainTextBytes.Length, saltBytes.Length);

            //Compute hash value of our plain text with appended salt.
            using (HashAlgorithm hash = new SHA512Managed())
            {
                byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

                //Convert result into a base64-encoded string and return it
                return (Convert.ToBase64String(hashBytes));
            }
        }

        /// <summary>
        /// Hash the plain text with the salt received and compares the hashed value with the has received and returns true if they are the same or false if they differs
        /// </summary>
        /// <param name="plainText">Plain text to be verified against the specified hash.</param>
        /// <param name="salt">Salt to be added to the password in the hashing process.</param>
        /// <param name="hash">Hash to compare with the hashed plain text and salt.</param>
        /// <returns>True if the plaint text and salt hash is the same as the hash received, otherwise false</returns>
        public bool VerifyHash(string plainText, string salt, string hash)
        {
            //Compute a new hash
            string newHash = ComputeHash(plainText, ref salt);

            //Return if the computed hash matches the received hash
            return (hash == newHash);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Converts a string into an array of bytes that when it is used to perform a reverse conversion gets a safe string to be passed in an URL.
        /// </summary>
        /// <param name="text">String to convert</param>
        /// <returns>The string converted as an array of bytes</returns>
        private byte[] StringToByteArray(string text)
        {
            //Validate arguments
            if (string.IsNullOrEmpty(text))
            {
                throw new CryptographicException("Tried to convert to array of bytes an empty or null text");
            }

            byte val;
            byte[] bytes = new byte[text.Length / 3];
            int i = 0;
            int j = 0;
            do
            {
                val = byte.Parse(text.Substring(i, 3));
                bytes[j++] = val;
                i += 3;
            }
            while (i < text.Length);

            //Return the resulting array of bytes
            return bytes;
        }

        /// <summary>
        /// Converts an array of bytes into a safe string to be passed in an URL.
        /// </summary>
        /// <param name="array">Array of bytes to convert</param>
        /// <returns>The array of bytes converted to a string</returns>
        private string ByteArrayToString(byte[] array)
        {
            byte val;
            string tempStr = string.Empty;
            for (int i = 0; i <= array.GetUpperBound(0); i++)
            {
                val = array[i];
                if (val < (byte)10)
                    tempStr += "00" + val.ToString();
                else if (val < (byte)100)
                    tempStr += "0" + val.ToString();
                else
                    tempStr += val.ToString();
            }

            //Return the resulting string
            return tempStr;
        }

        /// <summary>
        /// Generates a random array of bytes with a length between the minimum and maximum 
        /// </summary>
        /// <param name="minLength">Minimum length of the salt array</param>
        /// <param name="maxLength">Maximum length of the salt array</param>
        /// <returns>A random array of bytes to be used as password salt</returns>
        private byte[] CreateSalt(int minLength, int maxLength)
        {
            byte[] saltBytes;

            //Generate a random number for the salt length and create a byte array with that length
            Random random = new Random();
            int saltSize = random.Next(minLength, maxLength);
            saltBytes = new byte[saltSize];

            //Create the aleatory byte array
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(saltBytes);

            //Free resources
            rng.Dispose();

            //Return the result
            return saltBytes;
        }

        #endregion

        #region IDisposable implementation

        /// <summary>
        /// Variable used to detect redundant invocations
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// Main method for the IDisposable interface
        /// </summary>
        /// <param name="disposing">Signals if this objects is already being disposed</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //Free al the resources is this is the first call to this method and if this object is being disposed
                    if ((_encryptor != null))
                    {
                        _encryptor.Dispose();
                    }
                    if ((_decryptor != null))
                    {
                        _decryptor.Dispose();
                    }
                }
            }
            disposedValue = true;
        }

        /// <summary>
        /// Added code to implement the Disposable pattern
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
