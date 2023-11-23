using Newtonsoft.Json;
using OpenImis.ModulesV3.Helpers;
using System;

namespace OpenImis.ModulesV3.MasterDataModule.Models
{
    public class ItemModel
    {
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public string ItemPackage { get; set; }
        public decimal ItemPrice { get; set; }
        public string ItemCareType { get; set; }
        public short? ItemFrequency { get; set; }
        public byte ItemPatCat { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
    }
}