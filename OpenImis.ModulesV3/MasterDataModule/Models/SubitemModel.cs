using Newtonsoft.Json;
using OpenImis.ModulesV3.Helpers;
using System;

namespace OpenImis.ModulesV3.MasterDataModule.Models
{
    public class SubItemModel
    {
        public int id { get; set; }
        public int ItemId { get; set; }
        public int servicelinkedItem { get; set; }
        public int qty_provided { get; set; }
        public DateTime pcpDate { get; set; }
        public decimal price_asked { get; set; }
        public bool status { get; set; }
    }
}