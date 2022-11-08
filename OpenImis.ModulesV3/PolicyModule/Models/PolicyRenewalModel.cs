using Newtonsoft.Json;
using OpenImis.ModulesV3.Helpers;
using System;

namespace OpenImis.ModulesV3.PolicyModule.Models
{
    public class PolicyRenewalModel
    {
        public int RenewalId { get; set; }
        public string Officer { get; set; }
        public string CHFID { get; set; }
        public string ReceiptNo { get; set; }
        public string ProductCode { get; set; }
        public float Amount { get; set; }
        [JsonConverter(typeof(IsoDateSerializer))]
        public DateTime Date { get; set; }
        public bool Discontinue { get; set; }
        public int PayerId { get; set; }
        public string ControlNumber { get; set; }

        public Policy GetPolicy()
        {
            return new Policy()
            {
                RenewalId = RenewalId,
                Officer = Officer,
                CHFID = CHFID,
                ReceiptNo = ReceiptNo,
                ProductCode = ProductCode,
                Amount = Amount,
                Date = Date,
                Discontinue = Discontinue,
                PayerId = PayerId,
                ControlNumber = ControlNumber
            };
        }
    }
}
