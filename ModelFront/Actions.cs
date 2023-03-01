using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common;
using System.Text.Json;
using System.Text;
using ModelFront.Models;
using ModelFront.Requests;
using System.Web;

namespace ModelFront
{
    [ActionList]
    public class Actions
    {

        [Action]
        public Quality Estimate(AuthenticationCredentialsProvider authenticationCredentialsProvider, [ActionParameter] PredictionRequest predictionRequest)
        {
            var request = new Request
            {
                rows = new List<Row>() { new Row { original = predictionRequest.Original, translation = predictionRequest.Translation } }
            };

            var json = JsonSerializer.Serialize(request);

            var httpClient = new HttpClient();
            var builder = new UriBuilder("https://api.modelfront.com/v1/predict");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["sl"] = predictionRequest.SourceLanguage;
            query["tl"] = predictionRequest.TargetLanguage;
            query["token"] = authenticationCredentialsProvider.Value;
            if (predictionRequest.Engine != null) { query["engine"] = predictionRequest.Engine; }
            builder.Query = query.ToString();

            var httpRequest = new HttpRequestMessage()
            {
                RequestUri = builder.Uri,
                Method = HttpMethod.Post
            };

            httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = httpClient.Send(httpRequest);
            var responseString = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
            var result = JsonSerializer.Deserialize<Response>(responseString);

            return result.rows.First();
        }
    }
}