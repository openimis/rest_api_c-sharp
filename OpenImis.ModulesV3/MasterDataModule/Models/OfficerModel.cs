using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV3.MasterDataModule.Models
{
    public class OfficerModel
    {
        public int OfficerId { get; set; }
        public Guid OfficerUUID { get; set; }
        public string Code { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string Phone { get; set; }
        public int? LocationId { get; set; }
        public string OfficerIDSubst { get; set; }
        public string WorksTo { get; set; }
    }
}
