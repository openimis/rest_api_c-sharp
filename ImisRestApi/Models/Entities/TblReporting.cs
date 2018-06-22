using System;
using System.Collections.Generic;

namespace ImisRestApi.Models.Entities
{
    public partial class TblReporting
    {
        public int ReportingId { get; set; }
        public DateTime ReportingDate { get; set; }
        public int LocationId { get; set; }
        public int ProdId { get; set; }
        public int? PayerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RecordFound { get; set; }
    }
}
