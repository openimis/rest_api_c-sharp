using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.InsureeModule.Models
{
    public class DetailModel
    {
        public string ProductName { get; set; }
        public string ExpiryDate { get; set; }
        public string Status { get; set; }
        public float? DedType { get; set; }
        public decimal? Ded1 { get; set; }
        public decimal? Ded2 { get; set; }
        public decimal? Ceiling1 { get; set; }
        public decimal? Ceiling2 { get; set; }
        public string ProductCode { get; set; }
    }
}
