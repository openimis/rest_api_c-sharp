using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.MasterDataModule.Models
{
    public class PayerModel
    {
        public int PayerId { get; set; }
        public string PayerName { get; set; }
        public int? LocationId { get; set; }
    }
}
