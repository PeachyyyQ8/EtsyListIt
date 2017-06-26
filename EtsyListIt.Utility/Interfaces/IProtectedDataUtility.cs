using System.Security;

namespace EtsyListIt.Utility.Interfaces
{
    public interface IProtectedDataUtility
    {
        string EncryptString(SecureString workingDirectory);
    }
}