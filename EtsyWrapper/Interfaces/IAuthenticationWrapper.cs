using EtsyWrapper.DomainObjects;

namespace EtsyWrapper.Interfaces
{
    public interface IAuthenticationWrapper
    {
        TemporaryToken GetTemporaryCredentials(string apiKey, string sharedSecret, string[] permissions);

        void GetPermanentTokenCredentials(string apiKey, string sharedSecret, string validator, out string permanentToken, out string permanentSecret, string[] permissions);

        void SetAuthenticationToken(string apiKey, string sharedSecret, string[] permissions);

    }
}