using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Escape.Models
{
    public class ChfCoverage
    {
        public string OtherNames { get; set; }
        public string LastNames { get; set; }
        public string BirthDate { get; set; }

        public List<ChfCoverageProduct> CoverageProducts { get; set; }

    }

    public class ChfCoverageProduct
    {
        public string ProductCode { get; set; }
        public string PolicyValue { get; set; }
        public string EffectiveDate { get; set; }
        public string ExpiryDate { get; set; }
        public string Status { get; set; }

    }
}
