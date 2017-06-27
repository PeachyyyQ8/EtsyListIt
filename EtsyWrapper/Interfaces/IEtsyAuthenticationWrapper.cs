using EtsyWrapper.DomainObjects;

namespace EtsyWrapper.Interfaces
{
    public interface IEtsyAuthenticationWrapper
    {
        TemporaryToken GetTemporaryCredentials(string apiKey, string sharedSecret, string[] permissions);

        PermanentToken GetPermanentTokenCredentials(string apiKey, string sharedSecret, TemporaryToken tempToken,
            string validator);
    }
}