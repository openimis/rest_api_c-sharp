using System;
using System.Collections.Generic;

namespace ImisRestApi.Models.Entities
{
    public partial class TblExtracts
    {
        public int ExtractId { get; set; }
        public byte ExtractDirection { get; set; }
        public byte ExtractType { get; set; }
        public int ExtractSequence { get; set; }
        public DateTime ExtractDate { get; set; }
        public string ExtractFileName { get; set; }
        public string ExtractFolder { get; set; }
        public int LocationId { get; set; }
        public int? Hfid { get; set; }
        public decimal AppVersionBackend { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public long? RowId { get; set; }
    }
}
