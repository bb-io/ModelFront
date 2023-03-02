using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common;
using ModelFront.Models;
using App.ModelFront;
using RestSharp;
using App.ModelFront.Other;

namespace ModelFront
{
    [ActionList]
    public class Actions
    {
        [Action]
        public ResponseRow Predict(AuthenticationCredentialsProvider authenticationCredentialsProvider, [ActionParameter] PredictParameters input)
        {
            var client = new RestClient("https://api.modelfront.com");
            var request = new RestRequest("/v1/predict", Method.Post);
            request.AddQueryParameter("sl", input.SourceLanguage, false);
            request.AddQueryParameter("tl", input.TargetLanguage, false);
            request.AddQueryParameter("token", authenticationCredentialsProvider.Value, false);
            if (input.Engine != null) { request.AddQueryParameter("engine", input.Engine, false); }

            request.AddJsonBody(new Request
            {
                Rows = new List<Row>() { new Row { Original = input.Original, Translation = input.Translation } }
            });

            return client.Post<Response>(request).Rows.First();
        }

        [Action]
        public List<ResponseRow> PredictMany(AuthenticationCredentialsProvider authenticationCredentialsProvider, [ActionParameter] PredictManyParameters input)
        {
            var client = new RestClient("https://api.modelfront.com");
            var request = new RestRequest("/v1/predict", Method.Post);
            request.AddQueryParameter("sl", input.SourceLanguage, false);
            request.AddQueryParameter("tl", input.TargetLanguage, false);
            request.AddQueryParameter("token", authenticationCredentialsProvider.Value, false);
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