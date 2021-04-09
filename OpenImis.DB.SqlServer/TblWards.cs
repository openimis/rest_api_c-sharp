using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.DB.SqlServer
{
    public class TblWards
    {
        public int WardId { get; set; }
        public int? DistrictId { get; set; }
        public string WardCode { get; set; }
        public string WardName { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
    }
}
