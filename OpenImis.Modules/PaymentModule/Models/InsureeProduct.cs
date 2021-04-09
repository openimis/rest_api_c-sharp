using System;

namespace OpenImis.Modules.PaymentModule.Models
{
    public class InsureeProduct
    {
        public string InsureeNumber { get; set; }
        public string InsureeName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool PolicyActivated { get; set; }
        public decimal ExpectedProductAmount { get; set; }
    }
}
