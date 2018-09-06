using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ImisRestApi.Chanels.Payment.Models
{
    [XmlRoot("Gepg")]
    public class GepgBillMessage
    {
        public gepgBillSubReq gepgBillSubReq { get; set; }
        public string gepgSignature { get; set; }
    }

    [XmlRoot("Gepg")]
    public class GepgBillResponse
    {
        public gepgBillSubResp gepgBillSubResp { get; set; }
        public string gepgSignature { get; set; }
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

    public class BillHdr
    {
        public string SpCode { get; set; }
        public bool RtrRespFlg { get; set; }
    }

    public class BillTrxInf {
        public string BillId { get; set; }
        public int SubSpCode { get; set; }
        public string SpSysId { get; set; }
        public double BillAmt { get; set; }
        public double MiscAmt { get; set; }
        public DateTime BillExpDt { get; set; }
        public string PyrId { get; set; }
        public string PyrName { get; set; }
        public string BillDesc { get; set; }
        public DateTime BillGenDt { get; set; }
        public string BillGenBy { get; set; }
        public string BillApprBy { get; set; }
        public string PyrCellNum { get; set; }
        public string PyrEmail { get; set; }
        public string Ccy { get; set; }
        public double BillEqvAmt { get; set; }
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

   
    public class gepgBillSubResp
    {
        public List<BillTrxRespInf> BillTrxInf { get; set; }
    }

    [XmlRoot("BillTrxInf")]
    public class BillTrxRespInf
    {
        public string BillId { get; set; }
        public string TrxSts { get; set; }
        public int PayCntrNum { get; set; }
        public string TrxStsCode { get; set; }
    }

    public class gepgBillSubRespAck
    {
        public int TrxStsCode { get; set; }
    }

}
