using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using EtsyWrapper.DomainObjects;
using EtsyWrapper.Interfaces;
using Newtonsoft.Json;
using RestSharp;

namespace EtsyWrapper
{
    public class EtsyAuthenticationWrapper : IEtsyAuthenticationWrapper
    {
        private readonly IRestServiceWrapper _restServiceWrapper;
        private RestClient _restClient;

        public EtsyAuthenticationWrapper(IRestServiceWrapper restServiceWrapper)
        {
            _restServiceWrapper = restServiceWrapper;
            _restClient = restServiceWrapper.GetRestClient();
        }


        public TemporaryToken GetTemporaryCredentials(string apiKey, string sharedSecret, string[] permissions)
        {
            _restClient.Authenticator = _restServiceWrapper.GetAuthenticator(apiKey, sharedSecret);

            RestRequest restRequest = _restServiceWrapper.GetRestRequest("oauth/request_token", Method.POST);

            restRequest.AddParameter("scope", string.Join(" ", permissions));

            IRestResponse response = _restClient.Execute(restRequest);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            return ParseTemporaryToken(response.Content);
        }
        
        public PermanentToken GetPermanentTokenCredentials(string apiKey, string sharedSecret, string validator, string[] permissions)
        {
            throw new System.NotImplementedException();
        }

        public void SetAuthenticationToken(string apiKey, string sharedSecret, string[] permissions)
        {
            throw new System.NotImplementedException();
        }

        public static TemporaryToken ParseTemporaryToken(string queryString)
        {
            var args = new List<KeyValuePair<string, string>>();
            
            // remove anything other than query string from url
            if (queryString.Contains("?"))
            {
                queryString = queryString.Substring(queryString.IndexOf('?') + 1);
            }

            foreach (string arg in Regex.Split(queryString, "&"))
            {
                string[] singleArg = Regex.Split(arg, "=");
                if (singleArg.Length == 2)
                {
                    args.Add(new KeyValuePair<string, string>(singleArg[0], singleArg[1]));
                }
                else
                {
                    // only one key with no value specified in query string
                    args.Add(new KeyValuePair<string, string>(singleArg[0], string.Empty));
                }

            }

            return new TemporaryToken
            {
                LoginURL = args.First(x => x.Key == "login_url").Value,
                OAuthToken = args.First(x => x.Key == "oauth_token").Value,
                OAuthTokenSecret = args.First(x => x.Key == "oauth_token_secret").Value
            };
        }
    }
}