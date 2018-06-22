using System;
using System.Collections.Generic;

namespace ImisRestApi.Models.Entities
{
    public partial class TblPremium
    {
        public int PremiumId { get; set; }
        public int PolicyId { get; set; }
        public int? PayerId { get; set; }
        public decimal Amount { get; set; }
        public string Receipt { get; set; }
        public DateTime PayDate { get; set; }
        public string PayType { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public byte[] RowId { get; set; }
        public bool? IsPhotoFee { get; set; }
        public bool? IsOffline { get; set; }
        public int? ReportingId { get; set; }

        public TblPayer Payer { get; set; }
        public TblPolicy Policy { get; set; }
    }
}
