using System;
using System.Collections.Generic;

namespace OpenImis.RestApi.Models.Entities
{
    public partial class TblClaimAdmin
    {
        public TblClaimAdmin()
        {
            TblClaim = new HashSet<TblClaim>();
        }

        public int ClaimAdminId { get; set; }
        public string ClaimAdminCode { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public DateTime? Dob { get; set; }
        public string Phone { get; set; }
        public int? Hfid { get; set; }
        public DateTime? ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int? AuditUserId { get; set; }
        public byte[] RowId { get; set; }
        public string EmailId { get; set; }

        public TblHf Hf { get; set; }
        public ICollection<TblClaim> TblClaim { get; set; }
    }
}
