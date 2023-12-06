using Newtonsoft.Json;
using OpenImis.ModulesV3.Helpers;
using System;
using System.Collections.Generic;

namespace OpenImis.ModulesV3.ClaimModule.Models.RegisterClaim
{
    public class Claim
    {
        public ClaimDetails Details { get; set; }
        public List<Item> Items { get; set; }
        public List<Service> Services { get; set; }
        public List<ClaimAttachments> ClaimAttachments { get; set; }
    }

    public class ClaimDetails
    {
        [JsonConverter(typeof(IsoDateSerializer))]
        public DateTime ClaimDate { get; set; }
        public string HFCode { get; set; }
        public string ClaimAdmin { get; set; }
        public string ClaimCode { get; set; }
        public string CHFID { get; set; }
        [JsonConverter(typeof(IsoDateSerializer))]
        public DateTime StartDate { get; set; }
        [JsonConverter(typeof(IsoDateSerializer))]
        public DateTime EndDate { get; set; }
        public string ICDCode { get; set; }
        public string Comment { get; set; }
        public decimal? Total { get; set; }
        public string ICDCode1 { get; set; }
        public string ICDCode2 { get; set; }
        public string ICDCode3 { get; set; }
        public string ICDCode4 { get; set; }
        public string VisitType { get; set; }
        public string PrescriberType { get; set; }
    }

    public class Item
    {
        public string ItemCode { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal ItemQuantity { get; set; }
    }

    public class Service
    {
        public string ServiceCode { get; set; }
        public decimal ServicePrice { get; set; }
        public decimal ServiceQuantity { get; set; }
        public List<SubServicesItems> SubServicesItems { get; set; }
    }

    public class SubServicesItems
    {
        public string Code { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }
    }

    public class ClaimAttachments
    {
        public int? ClaimId { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string File { get; set; }
        public string Mime { get; set; }
    }
}
