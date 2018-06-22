using System;
using System.Collections.Generic;

namespace ImisRestApi.Models.Entities
{
    public partial class TblIcdcodes
    {
        public TblIcdcodes()
        {
            TblClaim = new HashSet<TblClaim>();
        }

        public int Icdid { get; set; }
        public string Icdcode { get; set; }
        public string Icdname { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public byte[] RowId { get; set; }

        public ICollection<TblClaim> TblClaim { get; set; }
    }
}
