using System.Collections.Specialized;
using System.Net;
using EtsyWrapper.DomainObjects;
using EtsyWrapper.Interfaces;
using RestSharp;

namespace EtsyWrapper
{
    public class AuthenticationWrapper : IAuthenticationWrapper
    {
        private readonly IRestServiceWrapper _restServiceWrapper;
        private RestClient _restClient;

        public AuthenticationWrapper(IRestServiceWrapper restServiceWrapper)
        {
            _restServiceWrapper = restServiceWrapper;
            _restClient = restServiceWrapper.GetRestClient();
        }


        public TemporaryToken GetTemporaryCredentials(string apiKey, string sharedSecret, string[] permissions)
        {
            _restClient.Authenticator = _restServiceWrapper.GetAuthenticator(apiKey, sharedSecret);

            RestRequest restRequest = _restServiceWrapper.GetRestRequest("oauth/request_token", Method.POST);

            restRequest.AddParameter("scope", permissions);

            IRestResponse response = _restClient.Execute(restRequest);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            //TODO  figure out how to do this!
           // NameValueCollection queryString = response.
                //System.Web.HttpUtility.ParseQueryString(response.Content);

            //oAuthToken = queryString["oauth_token"];
            //oauthTokenSecret = queryString["oauth_token_secret"];

            //return queryString["login_url"];

            return new TemporaryToken();
        }

        public void GetPermanentTokenCredentials(string apiKey, string sharedSecret, string validator, out string permanentToken,
            out string permanentSecret, string[] permissions)
        {
            throw new System.NotImplementedException();
        }

        public void SetAuthenticationToken(string apiKey, string sharedSecret, string[] permissions)
        {
            throw new System.NotImplementedException();
        }
    }
}