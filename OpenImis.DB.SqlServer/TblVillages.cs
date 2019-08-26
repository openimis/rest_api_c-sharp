using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.DB.SqlServer
{
    public class TblVillages
    {
        public int VillageId { get; set; }
        public int? WardId { get; set; }
        public string VillageCode { get; set; }
        public string VillageName { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
    }
}
