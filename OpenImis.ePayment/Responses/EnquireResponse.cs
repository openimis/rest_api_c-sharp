using System.Data;

namespace OpenImis.ePayment.Responses
{
    public class EnquireResponse
    {
        public string productCode { get; set; }
        public string status { get; set; }
        public string insureeName { get; set; }
    }
}