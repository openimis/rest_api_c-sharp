using OpenImis.ePayment.Models.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenImis.ePayment.Escape.Payment.Models
{
    [XmlRoot("Gepg")]
    public class GePGPaymentCancelRequest
    {
        public gepgBillCanclReq gepgBillCanclReq { get; set; }
        public string gepgSignature { get; set; }
    }

    [XmlRoot("Gepg")]
    public class GePGPaymentCancelResponse
    {
        public gepgBillCanclResp gepgBillCanclResp { get; set; }
        public string gepgSignature { get; set; }
    }

    public class gepgBillCanclReq
    {
        public string SpCode { get; set; }
        public string SpSysId { get; set; }
        public string BillId { get; set; }
    }

    [XmlRoot("gepgBillCanclResp")]
    public class gepgBillCanclResp
    {
        [XmlElement("BillCanclTrxDt")]
        public BillCanclTrxDt[] BillCanclTrxDt { get; set; }
    }

    public class BillCanclTrxDt
    {
        public string BillId { get; set; }
        public string TrxSts { get; set; }
        public string TrxStsCode { get; set; }
    }

    public static class TrxSts
    {
        public static string Success = "GS";
        public static string Failure = "GF";
    }



}
