using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common;
using ModelFront.Models;
using RestSharp;
using App.ModelFront.Other;
using Apps.ModelFront.Parameters;
using Blackbird.Applications.Sdk.Common.Actions;
using Apps.ModelFront;

namespace ModelFront
{
    [ActionList]
    public class Actions
    {
        [Action("Predict", Description = "Get prediction data for a single segment")]
        public ResponseRow Predict(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, [ActionParameter] PredictParameters input)
        {
            var client = new ModelFrontClient();
            var request = new ModelFrontRequest("/predict", Method.Post, authenticationCredentialsProviders);
            request.AddQueryParameter("sl", input.SourceLanguage, false);
            request.AddQueryParameter("tl", input.TargetLanguage, false);
            if (input.Engine != null) { request.AddQueryParameter("engine", input.Engine, false); }

            request.AddJsonBody(new Request
            {
                Rows = new List<Row>() { new Row { Original = input.Original, Translation = input.Translation } }
            });

            return client.Post<Response>(request).Rows.First();
        }

        [Action("Predict multiple", Description = "Get prediction data for multiple segments")]
        public List<ResponseRow> PredictMany(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, [ActionParameter] PredictManyParameters input)
        {
            var client = new ModelFrontClient();
            var request = new ModelFrontRequest("/predict", Method.Post, authenticationCredentialsProviders);
            request.AddQueryParameter("sl", input.SourceLanguage, false);
            request.AddQueryParameter("tl", input.TargetLanguage, false);
            if (input.Engine != null) { request.AddQueryParameter("engine", input.Engine, false); }

            var analyzedRows = input.Segments.ChunkBy(30).SelectMany(rows =>
            {
                request.AddJsonBody(new Request { Rows = rows });
                return client.Post<Response>(request).Rows;
            });

            return analyzedRows.ToList();
        }
    }
}