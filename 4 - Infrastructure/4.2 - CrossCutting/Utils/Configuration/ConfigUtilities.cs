using Moviit.Infrastructure.CrossCutting.Utils.Cryptography;
using Moviit.Infrastructure.CrossCutting.Utils.Exceptions;
using System;
using System.Configuration;

namespace Moviit.Infrastructure.CrossCutting.Utils.Configuration
{

    /// <summary>
    /// Auxiliary class for configuration management.
    /// </summary>
    public static class ConfigUtilities
    {

        #region Public Methods

        /// <summary>
        /// Saves or updates in the configuration (in appSettings) the key and value passed.
        /// </summary>
        /// <param name="key">Key to use to save the value</param>
        /// <param name="value">Value to be saved (it will be trimmed)</param>
        /// <param name="encript">True if the value must be encrypted, false otherwise</param>
        public static void SetKey(string key, string value, bool encript = false)
        {
            //Open the configuration
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            try
            {
                //Encrypt the value if needed
                string strValue = value;
                if (encript)
                {
                    using (CryptoUtilities crypto = new CryptoUtilities())
                    {
                        strValue = crypto.EncryptToString(value);
                    }
                }

                //Add the key-value in the configuration
                config.AppSettings.Settings.Remove(key);
                config.Save(ConfigurationSaveMode.Modified);
                config.AppSettings.Settings.Add(key, strValue.Trim());
                config.Save(ConfigurationSaveMode.Modified);

                //Refresh the configuration to take the new key-value
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                throw new SystemConfigurationException(ex, "The key {0} could not be saved in the configuration", key);
            }
        }

        /// <summary>
        /// Returns from the configuration (from appSettings) the value corresponding to the key passed by parameter.
        /// </summary>
        /// <param name="key">Key of the value to get</param>
        /// <param name="isEncripted">True if the value is encrypted, false otherwise. False by default</param>
        /// <param name="defaultValue">The value to return if the key is not present. If this parameter is empty then when the key is not present an exception will be thrown. Empty by default</param>
        /// <returns>The value corresponding to the key passed by parameter</returns>
        public static string GetValue(string key, bool isEncripted = false, string defaultValue = "")
        {
            string strVal = string.Empty;

            //Open the configuration
            try
            {
                //Get the value from the configuration
                strVal = ConfigurationManager.AppSettings[key];

                if (strVal == null && string.IsNullOrWhiteSpace(defaultValue))
                {
                    throw new SystemConfigurationException("The key {0} could not be read from the configuration", key);
                }
                else if (strVal == null)
                {
                    strVal = defaultValue;
                }

                //Decrypt the value if needed
                if (isEncripted && !string.IsNullOrWhiteSpace(strVal))
                {
                    using (CryptoUtilities crypto = new CryptoUtilities())
                    {
                        strVal = crypto.DecryptString(strVal.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SystemConfigurationException(ex, "The key {0} could not be read from the configuration", key);
            }

            //Return the value
            return strVal;
        }

        /// <summary>
        /// Returns from the configuration the connection string passed by parameter.
        /// </summary>
        /// <param name="name">Name of the connection string passed by parameter. False by default</param>
        /// <param name="isEncripted">True if the connection string is encrypted, false otherwise</param>
        /// <returns>The connection string passed by parameter</returns>
        public static string GetConnectionString(string name, bool isEncripted = false)
        {
            string connectionString = string.Empty;

            //Open the configuration
            try
            {
                //Get the connection string from the configuration
                connectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;

                if (connectionString == null)
                {
                    throw new SystemConfigurationException("The connection string {0} could not be read from the configuration", name);
                }

                //Decrypt it if needed
                if (isEncripted && !string.IsNullOrWhiteSpace(connectionString))
                {
                    using (CryptoUtilities crypto = new CryptoUtilities())
                    {
                        connectionString = crypto.DecryptString(connectionString.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SystemConfigurationException(ex, "The connection string {0} could not be read from the configuration", name);
            }

            //Return the connection string
            return connectionString;
        }

        #endregion

    }
}
