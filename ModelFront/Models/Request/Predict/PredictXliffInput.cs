using Apps.ModelFront.DataSourceHandlers;
using Apps.ModelFront.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModelFront.Models.Request.Predict
{
    public class PredictXliffInput
    {
        public FileReference File { get; set; }

        public float? Threshold { get; set; }

        [DataSource(typeof(ConditionDataHandler))]
        public string? Condition { get; set; }

        [Display("New Target State")]
        [DataSource(typeof(XliffStateDataHandler))]
        public string? State { get; set; }
    }
}
