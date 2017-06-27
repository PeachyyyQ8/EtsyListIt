using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace EtsyWrapper.DomainObjects
{
    public class TemporaryToken
    {
        [DataMember(Name = "oauth_token")]
        public string OAuthToken { get; set; }

        [DataMember(Name = "oauth_token_secret")]
        public string OAuthTokenSecret { get; set; }

        [DataMember(Name = "login_url")]
        public string LoginURL { get; set; }

        
    }
}