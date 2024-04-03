using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModelFront.DataSourceHandlers.EnumDataHandlers
{
    public class ConditionDataHandler : EnumDataHandler
    {
        protected override Dictionary<string, string> EnumValues => new()
        {
            { ">", "Quality is above threshold" },
            { ">=", "Quality is above or equal threshold" },
            { "=", "Quality is same as threshold" },
            { "<", "Quality is below threshold" },
            { "<=", "Quality is below or equal threshold" }
        };
    }
}
