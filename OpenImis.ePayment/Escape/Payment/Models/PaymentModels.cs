using OpenImis.ePayment.ImisAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace OpenImis.ePayment.Escape.Payment.Models
{
    [XmlRoot("Gepg")]
    public class GepgPaymentMessage
    {
        // public gepgPmtSpInfo gepgPmtSpInfo { get; set; }
        [XmlArray("gepgPmtSpInfo")]
        [XmlArrayItem("PymtTrxInf")]
        public PymtTrxInf[] PymtTrxInf { get; set; }
        public string gepgSignature { get; set; }
    }

    [XmlRoot("Gepg")]
    public class GepgPaymentAck
    {
        public gepgPmtSpInfoAck gepgPmtSpInfoAck { get; set; }
        public string gepgSignature { get; set; }
    }

    public class gepgPmtSpInfo : GePGValidatedResponseModel
    {
        [XmlArray]
        public List<PymtTrxInf> PymtTrxInf { get; set; }
        
    }
    public class PymtTrxInf
    {    
        public string TrxId { get; set; }
        public string SpCode { get; set; }
        public string PayRefId { get; set; }
        public string BillId { get; set; }
        public string PayCtrNum { get; set; }
        public decimal BillAmt { get; set; }
        public decimal PaidAmt { get; set; }
        public int BillPayOpt { get; set; }
        public string CCy { get; set; }
        public string TrxDtTm { get; set; }
        public string UsdPayChnl { get; set; }
        public string PyrCellNum { get; set; }
        public string PyrEmail { get; set; }
        public string PyrName { get; set; }      
        public string PspReceiptNumber { get; set; }
        public string PspName { get; set; }
        public string CtrAccNum { get; set; }
        
    }

    public class gepgPmtSpInfoAck
    {
        public int TrxStsCode { get; set; }
    }
}