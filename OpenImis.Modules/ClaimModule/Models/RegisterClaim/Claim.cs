using System.Collections.Generic;

namespace OpenImis.Modules.ClaimModule.Models.RegisterClaim
{
    public class Claim
    {
        public ClaimDetails Details { get; set; }
        public List<Item> Items { get; set; }
        public List<Service> Services { get; set; }
    }

    public class ClaimDetails
    {
        public string ClaimDate { get; set; }
        public string HFCode { get; set; }
        public string ClaimAdmin { get; set; }
        public string ClaimCode { get; set; }
        public string CHFID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ICDCode { get; set; }
        public string Comment { get; set; }
        public string Total { get; set; }
        public string ICDCode1 { get; set; }
        public string ICDCode2 { get; set; }
        public string ICDCode3 { get; set; }
        public string ICDCode4 { get; set; }
        public string VisitType { get; set; }
    }

    public class Item
    {
        public string ItemCode { get; set; }
        public string ItemPrice { get; set; }
        public string ItemQuantity { get; set; }
    }

    public class Service
    {
        public string ServiceCode { get; set; }
        public string ServicePrice { get; set; }
        public string ServiceQuantity { get; set; }
    }
}
