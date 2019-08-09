using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.DB.SqlServer
{
    public class TblDistricts
    {
        public int DistrictId { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
    }
}
