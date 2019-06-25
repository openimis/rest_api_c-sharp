using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.CoverageModule.Models
{
    public class CoverageModel
    {
        public string OtherNames { get; set; }
        public string LastNames { get; set; }
        public string BirthDate { get; set; }
        public List<CoverageProduct> CoverageProducts { get; set; }
    }
}
