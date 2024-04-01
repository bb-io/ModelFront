using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModelFront.Models.Response.Predict
{
    public class PredictXliffResponse
    {
        public FileReference File { get; set; }

        [Display("Average Quality")]
        public float AverageQuality { get; set; }

        [Display("Average Risk")]
        public float AverageRisk { get; set; }
    }
}
