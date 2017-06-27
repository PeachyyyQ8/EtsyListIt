using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using EtsyWrapper.DomainObjects;
using EtsyWrapper.Interfaces;
using RestSharp;

namespace EtsyWrapper
{
    public class EtsyAuthenticationWrapper : IEtsyAuthenticationWrapper
    {
        private readonly IRestServiceWrapper _restServiceWrapper;
        private readonly RestClient _restClient;

        public EtsyAuthenticationWrapper(IRestServiceWrapper restServiceWrapper)
        {
            _restServiceWrapper = restServiceWrapper;
            _restClient = restServiceWrapper.GetRestClient();
        }


        public TemporaryToken GetTemporaryCredentials(string apiKey, string sharedSecret, string[] permissions)
        {
            _restClient.Authenticator = _restServiceWrapper.GetAuthenticatorForRequestToken(apiKey, sharedSecret);

            RestRequest restRequest = _restServiceWrapper.GetRestRequest("oauth/request_token", Method.POST);

            restRequest.AddParameter("scope", string.Join(" ", permissions));

            IRestResponse response = _restClient.Execute(restRequest);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            var args = ParseQueryString(response.Content);

            return new TemporaryToken
            {
                LoginURL = Uri.UnescapeDataString(args.First(x => x.Key == "login_url").Value),
                OAuthToken = args.First(x => x.Key == "oauth_token").Value,
                OAuthTokenSecret = args.First(x => x.Key == "oauth_token_secret").Value
            };
        }
        
        public PermanentToken GetPermanentTokenCredentials(string apiKey, string sharedSecret, TemporaryToken tempToken, string validator)
        {
            //consumerKey is the appKey you got when you registered your app, same for sharedSecret
            _restClient.Authenticator = _restServiceWrapper.GetAuthenticatorForAccessToken(apiKey, sharedSecret, tempToken, validator);

            RestRequest restRequest = _restServiceWrapper.GetRestRequest("oauth/access_token", Method.GET);

            IRestResponse irestResponse = _restClient.Execute(restRequest);

            var args = ParseQueryString(irestResponse.Content);

            var permanentToken = new PermanentToken
            {
                APIKey = apiKey,
                SharedSecret = sharedSecret,
                TokenID = args.First(x => x.Key == "oauth_token").Value,
                TokenSecret = args.First(x => x.Key == "oauth_token_secret").Value
            };

            
            if (string.IsNullOrEmpty(permanentToken.TokenID) || string.IsNullOrEmpty(permanentToken.TokenSecret))
            {
                throw new NullReferenceException("Unable to retrieve permanent auth token.  Please check your credentials and try again.");
            }

            return permanentToken;
        }
        
        public static List<KeyValuePair<string, string>> ParseQueryString(string queryString)
        {
            var args = new List<KeyValuePair<string, string>>();
            
            // remove anything other than query string from url
            if (queryString.Contains("?"))
            {
                queryString = queryString.Substring(queryString.IndexOf('?') + 1);
            }

            foreach (var arg in Regex.Split(queryString, "&"))
            {
                var singleArg = Regex.Split(arg, "=");
                args.Add(singleArg.Length == 2
                    ? new KeyValuePair<string, string>(singleArg[0], singleArg[1])
                    : new KeyValuePair<string, string>(singleArg[0], string.Empty));
            }

            return args;
        }
    }
}