using System.Collections.Generic;

namespace OpenImis.ModulesV2.ClaimModule.Models.RegisterClaim
{
    public class Claim
    {
        public Details Details { get; set; }
        public List<Item> Items { get; set; }
        public List<Service> Services { get; set; }
    }

    public class Details
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
        public float Total { get; set; }
        public string ICDCode1 { get; set; }
        public string ICDCode2 { get; set; }
        public string ICDCode3 { get; set; }
        public string ICDCode4 { get; set; }
        public int VisitType { get; set; }
    }

    public class Item
    {
        public string ItemCode { get; set; }
        public float ItemPrice { get; set; }
        public int ItemQuantity { get; set; }
    }

    public class Service
    {
        public string ServiceCode { get; set; }
        public float ServicePrice { get; set; }
        public int ServiceQuantity { get; set; }
    }
}
