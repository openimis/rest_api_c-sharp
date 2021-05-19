using OpenImis.ePayment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Escape.Models
{
    public class ChfContribution : Contribution
    {
        public int language { get; set; }
        public string msisdn { get; set; }
    }
}
