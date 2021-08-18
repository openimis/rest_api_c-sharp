using OpenImis.ePayment.Models.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenImis.ePayment.Escape.Payment.Models
{
    [XmlRoot("Gepg")]
    public class GepgBillMessage
    {
        public gepgBillSubReq gepgBillSubReq { get; set; }
        public string gepgSignature { get; set; }
    }

    [XmlRoot("Gepg")]
    public class GepgBillResponse: GePGValidatedResponseModel
    {     
        [XmlArray("gepgBillSubResp")]
        [XmlArrayItem("BillTrxInf")]
        public BillTrxRespInf[] BillTrxInf { get; set; }
        
    }

    [XmlRoot("Gepg")]
    public class GepgBillResponseAck
    {
        public gepgBillSubRespAck gepgBillSubRespAck { get; set; }
        public string gepgSignature { get; set; }
    }

    public class gepgBillSubReq
    {
        public BillHdr BillHdr { get; set; }
        public BillTrxInf BillTrxInf { get; set; }
    }

    [XmlRoot("gepgBillSubReq")]
    public class GepgBulkBillSubReq
    {
        public BillHdr BillHdr { get; set; }
        public List<BillTrxInf> BillTrxInf { get; set; }
    }

    public class BillHdr
    {
        public string SpCode { get; set; }
        public bool RtrRespFlg { get; set; }
    }

    public class BillTrxInf {
        public int BillId { get; set; }
        public int SubSpCode { get; set; }
        public string SpSysId { get; set; }
        public decimal BillAmt { get; set; }
        public double MiscAmt { get; set; }
        public string BillExprDt { get; set; }
        public string PyrId { get; set; }
        public string PyrName { get; set; }
        public string BillDesc { get; set; }
        public string BillGenDt { get; set; }
        public string BillGenBy { get; set; }
        public string BillApprBy { get; set; }
        public string PyrCellNum { get; set; }
        public string PyrEmail { get; set; }
        public string Ccy { get; set; }
        public decimal BillEqvAmt { get; set; }
        public bool RemFlag { get; set; }
        public int BillPayOpt { get; set; }
        public List<BillItem> BillItems { get; set; }
    }

    public class BillItem
    {
        public string BillItemRef { get; set; }
        public string UseItemRefOnPay { get; set; }
        public double BillItemAmt { get; set; }
        public double BillItemEqvAmt { get; set; }
        public double BillItemMiscAmt { get; set; }
        public string GfsCode { get; set; }
    }

    public class gepgBillSubReqAck
    {
        public int TrxStsCode { get; set; }
    }

    [XmlRoot("gepgBillSubResp")]
    public class gepgBillSubResp : GePGValidatedResponseModel
    {
        [XmlElement("BillTrxInf")]
        public BillTrxRespInf[] BillTrxInf { get; set; }
    }

    public class BillTrxRespInf
    {
        public int BillId { get; set; }
        public string PayCntrNum { get; set; }
        public string TrxSts { get; set; }
        public string TrxStsCode { get; set; }
    }

    public class gepgBillSubRespAck
    {
        public int TrxStsCode { get; set; }
    }

    [XmlRoot("Gepg")]
    public class GepgBulkBillMessage
    {
        public GepgBulkBillSubReq gepgBillSubReq { get; set; }
        public string gepgSignature { get; set; }
    }

}
