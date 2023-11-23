using Newtonsoft.Json;
using OpenImis.ModulesV3.Helpers;
using System;

namespace OpenImis.ModulesV3.MasterDataModule.Models
{
    public class SubServiceModel
    {
        public int id { get; set; }
        public int ServiceId { get; set; }
        public int servicelinkedService { get; set; }
        public int qty_provided { get; set; }
        public DateTime scpDate { get; set; }
        public decimal price_asked { get; set; }
        public bool status { get; set; }
    }
}