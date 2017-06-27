using System;
using System.Configuration;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using EtsyListIt.Utility.Extensions;
using EtsyListIt.Utility.Interfaces;

namespace EtsyListIt.Utility
{
    public class SettingsUtility : ISettingsUtility
    {
        readonly byte[] _entropy = Encoding.Unicode.GetBytes("EtsyListIt Application");

        public string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public string GetEncryptedAppSetting(string settingKey)
        {
            var value = GetAppSetting(settingKey);
            return !value.IsNullOrEmpty() ? DecryptString(value).ToInsecureString() : value;
        }

        public void SetAppSetting(string key, string value)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            var settings = configFile.AppSettings.Settings;

            if (settings[key] == null || settings[key].ToString().Trim() == string.Empty)
            {

                settings.Add(key, value);
            }
            else
            {
                settings[key].Value = value;
            }

            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }

        public void SetAppSettingWithEncryption(string key, string value)
        {
            

            var encryptedValue = EncryptString(value.ToSecureString());

            SetAppSetting(key, encryptedValue);
        }

        public string EncryptString(SecureString input)
        {
            byte[] encryptedData = ProtectedData.Protect(Encoding.Unicode.GetBytes(input.ToInsecureString()), _entropy, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        public SecureString DecryptString(string encryptedData)
        {
            var decryptedData = ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), _entropy,
                DataProtectionScope.CurrentUser);
            return Encoding.Unicode.GetString(decryptedData).ToSecureString();
        }
    }
}
