using System;
using System.Collections.Generic;

namespace ImisRestApi.Models.Entities
{
    public partial class TblPolicy
    {
        public TblPolicy()
        {
            TblClaimDedRem = new HashSet<TblClaimDedRem>();
            TblInsureePolicy = new HashSet<TblInsureePolicy>();
            TblPolicyRenewals = new HashSet<TblPolicyRenewals>();
            TblPremium = new HashSet<TblPremium>();
        }

        public int PolicyId { get; set; }
        public int FamilyId { get; set; }
        public DateTime EnrollDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public byte? PolicyStatus { get; set; }
        public decimal? PolicyValue { get; set; }
        public int ProdId { get; set; }
        public int? OfficerId { get; set; }
        public string PolicyStage { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public byte[] RowId { get; set; }
        public bool? IsOffline { get; set; }

        public TblFamilies Family { get; set; }
        public TblOfficer Officer { get; set; }
        public TblProduct Prod { get; set; }
        public ICollection<TblClaimDedRem> TblClaimDedRem { get; set; }
        public ICollection<TblInsureePolicy> TblInsureePolicy { get; set; }
        public ICollection<TblPolicyRenewals> TblPolicyRenewals { get; set; }
        public ICollection<TblPremium> TblPremium { get; set; }
    }
}
