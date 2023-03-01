using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelFront.Requests
{
    public class PredictionRequest
    {
        public string Original { get; set; }
        public string Translation { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public string Engine { get; set; }

    }
}
