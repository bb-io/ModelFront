using Blackbird.Applications.Sdk.Common.Authentication;
using ModelFront.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModelFront
{
    public class ModelFrontRequest : RestRequest
    {
        public ModelFrontRequest(string endpoint, Method method, IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders) : base(endpoint, method)
        {
            var token = authenticationCredentialsProviders.First(p => p.KeyName == "token").Value;
            this.AddQueryParameter("token", token, false);
        }
    }
}
