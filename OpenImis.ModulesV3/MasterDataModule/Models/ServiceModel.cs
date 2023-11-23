using Newtonsoft.Json;
using OpenImis.ModulesV3.Helpers;
using System;

namespace OpenImis.ModulesV3.MasterDataModule.Models
{
    public class ServiceModel
    {
        public int ServiceId { get; set; }
        public string ServCode { get; set; }
        public string ServName { get; set; }
        public string ServType { get; set; }
        public string ServLevel { get; set; }
        public decimal ServPrice { get; set; }
        public string ServCareType { get; set; }
        public short? ServFrequency { get; set; }
        public byte ServPatCat { get; set; }
        public DateTime? ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public string ServCategory { get; set; }
        public string ServPackageType { get; set; }
    }
}