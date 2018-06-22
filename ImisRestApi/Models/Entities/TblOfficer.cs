using System;
using System.Collections.Generic;

namespace ImisRestApi.Models.Entities
{
    public partial class TblOfficer
    {
        public TblOfficer()
        {
            InverseOfficerIdsubstNavigation = new HashSet<TblOfficer>();
            TblOfficerVillages = new HashSet<TblOfficerVillages>();
            TblPolicy = new HashSet<TblPolicy>();
            TblPolicyRenewals = new HashSet<TblPolicyRenewals>();
        }

        public int OfficerId { get; set; }
        public string Code { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public DateTime? Dob { get; set; }
        public string Phone { get; set; }
        public int? LocationId { get; set; }
        public int? OfficerIdsubst { get; set; }
        public DateTime? WorksTo { get; set; }
        public string Veocode { get; set; }
        public string VeolastName { get; set; }
        public string VeootherNames { get; set; }
        public DateTime? Veodob { get; set; }
        public string Veophone { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public byte[] RowId { get; set; }
        public string EmailId { get; set; }
        public bool? PhoneCommunication { get; set; }
        public string Permanentaddress { get; set; }

        public TblLocations Location { get; set; }
        public TblOfficer OfficerIdsubstNavigation { get; set; }
        public ICollection<TblOfficer> InverseOfficerIdsubstNavigation { get; set; }
        public ICollection<TblOfficerVillages> TblOfficerVillages { get; set; }
        public ICollection<TblPolicy> TblPolicy { get; set; }
        public ICollection<TblPolicyRenewals> TblPolicyRenewals { get; set; }
    }
}
