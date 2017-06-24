using EtsyWrapper.Interfaces;
using RestSharp;
using RestSharp.Authenticators;

namespace EtsyWrapper
{
    public class RestServiceWrapper : IRestServiceWrapper
    {
        public RestClient GetRestClient()
        {
            return new RestClient("");
        }

        public IAuthenticator GetAuthenticator(string apiKey, string sharedSecret)
        {
            return OAuth1Authenticator.ForRequestToken(apiKey, sharedSecret, "oob");
        }

        public RestRequest GetRestRequest(string request, Method method)
        {
            return new RestRequest(request, method);
        }
        
    }
}