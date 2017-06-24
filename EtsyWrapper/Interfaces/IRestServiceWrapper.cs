using RestSharp;
using RestSharp.Authenticators;

namespace EtsyWrapper.Interfaces
{
    public interface IRestServiceWrapper
    {
        RestClient GetRestClient();
        IAuthenticator GetAuthenticator(string apiKey, string sharedSecretS);
        RestRequest GetRestRequest(string oauthRequestToken, Method post);
    }
}