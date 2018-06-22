using System;
using System.Collections.Generic;

namespace ImisRestApi.Models.Entities
{
    public partial class TblPolicyRenewals
    {
        public TblPolicyRenewals()
        {
            TblPolicyRenewalDetails = new HashSet<TblPolicyRenewalDetails>();
        }

        public int RenewalId { get; set; }
        public DateTime RenewalPromptDate { get; set; }
        public DateTime RenewalDate { get; set; }
        public int? NewOfficerId { get; set; }
        public string PhoneNumber { get; set; }
        public byte Smsstatus { get; set; }
        public int InsureeId { get; set; }
        public int PolicyId { get; set; }
        public int NewProdId { get; set; }
        public byte? RenewalWarnings { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int? AuditCreateUser { get; set; }
        public int? ResponseStatus { get; set; }
        public DateTime? ResponseDate { get; set; }

        public TblInsuree Insuree { get; set; }
        public TblOfficer NewOfficer { get; set; }
        public TblProduct NewProd { get; set; }
        public TblPolicy Policy { get; set; }
        public ICollection<TblPolicyRenewalDetails> TblPolicyRenewalDetails { get; set; }
    }
}
