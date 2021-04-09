using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.MasterDataModule.Models
{
    public class PayerModel
    {
        public int PayerId { get; set; }
        public string PayerName { get; set; }
        public int? LocationId { get; set; }
    }
}
