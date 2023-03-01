using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelFront.Models
{
    public class Response
    {
        public string status { get; set; }
        public List<Quality> rows { get; set; }
    }
}
