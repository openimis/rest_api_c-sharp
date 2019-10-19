using System;

namespace OpenImis.ModulesV2.ReportModule.Models
{
    public class SnapshotResponseModel
    {
        public int Active { get; set; }
        public int Expired { get; set; }
        public int Idle { get; set; }
        public int Suspended { get; set; }

    }
}
