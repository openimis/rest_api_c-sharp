using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ePayment.Models.Payment
{
    public class ProductDetailsVM
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public decimal Lumpsum { get; set; }
        public string AccCodePremiums { get; set; }
    }
}
