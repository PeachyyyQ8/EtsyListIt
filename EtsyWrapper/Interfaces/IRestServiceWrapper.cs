using EtsyWrapper.DomainObjects;
using RestSharp;
using RestSharp.Authenticators;

namespace EtsyWrapper.Interfaces
{
    public interface IRestServiceWrapper
    {
        RestClient GetRestClient();
        IAuthenticator GetAuthenticatorForRequestToken(string apiKey, string sharedSecretS);
        RestRequest GetRestRequest(string oauthRequestToken, Method post);
        IAuthenticator GetAuthenticatorForAccessToken(string apiKey, string sharedSecret, TemporaryToken tempToken, string verifier);
        
    }
}