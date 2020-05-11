using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Escape.Models
{
    public class ChfGetCoverageModel
    {
        public string insureeNumber { get; set; }
        public int language { get; set; }
        public string msisdn { get; set; }
    }
}
