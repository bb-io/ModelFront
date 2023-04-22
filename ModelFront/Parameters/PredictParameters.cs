using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModelFront.Parameters
{
    public class PredictParameters
    {
        public string Original { get; set; }
        public string Translation { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public string Engine { get; set; }

    }
}
