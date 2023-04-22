using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModelFront
{
    public class ModelFrontClient : RestClient
    {
        public ModelFrontClient() : base(new RestClientOptions() { ThrowOnAnyError = true, BaseUrl = new Uri("https://api.modelfront.com/v1") }) { }
    }
}
