using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV3.MasterDataModule.Models
{
    public class HFModel
    {
        public int HFID { get; set; }
        public string HFCode { get; set; }
        public string HFName { get; set; }
        public int LocationId { get; set; }
        public string HFLevel { get; set; }
    }
}
