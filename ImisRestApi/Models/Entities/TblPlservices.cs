using System;
using System.Collections.Generic;

namespace ImisRestApi.Models.Entities
{
    public partial class TblPlservices
    {
        public TblPlservices()
        {
            TblHf = new HashSet<TblHf>();
            TblPlservicesDetail = new HashSet<TblPlservicesDetail>();
        }

        public int PlserviceId { get; set; }
        public string PlservName { get; set; }
        public DateTime DatePl { get; set; }
        public int? LocationId { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public byte[] RowId { get; set; }

        public TblLocations Location { get; set; }
        public ICollection<TblHf> TblHf { get; set; }
        public ICollection<TblPlservicesDetail> TblPlservicesDetail { get; set; }
    }
}
