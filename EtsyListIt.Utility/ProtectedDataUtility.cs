using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using EtsyListIt.Utility.Extensions;
using EtsyListIt.Utility.Interfaces;

namespace EtsyListIt.Utility
{
    public class ProtectedDataUtility : IProtectedDataUtility
    {
        private readonly byte[] _entropy = Encoding.Unicode.GetBytes("EtsyListIt Application");

        public string EncryptString(SecureString input)
        {
            byte[] encryptedData = ProtectedData.Protect(Encoding.Unicode.GetBytes(input.ToInsecureString()), _entropy, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        public SecureString DecryptString(string encryptedData)
        {
            try
            {
                var decryptedData = ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), _entropy, DataProtectionScope.CurrentUser);
                return Encoding.Unicode.GetString(decryptedData).ToSecureString();
            }
            catch
            {
                return new SecureString();
            }
        }
    }
}