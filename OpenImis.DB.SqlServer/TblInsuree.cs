using System;
using System.Collections.Generic;

namespace OpenImis.DB.SqlServer
{
    public partial class TblInsuree
    {
        public TblInsuree()
        {
            TblClaim = new HashSet<TblClaim>();
            TblClaimDedRem = new HashSet<TblClaimDedRem>();
            TblFamilies = new HashSet<TblFamilies>();
            TblHealthStatus = new HashSet<TblHealthStatus>();
            TblInsureePolicy = new HashSet<TblInsureePolicy>();
            TblPolicyRenewalDetails = new HashSet<TblPolicyRenewalDetails>();
            TblPolicyRenewals = new HashSet<TblPolicyRenewals>();
        }

        public int InsureeId { get; set; }
        public Guid InsureeUUID { get; set; }
        public int FamilyId { get; set; }
        public string Chfid { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string ArabLastName { get; set; }
        public string ArabOtherNames { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public string Marital { get; set; }
        public bool IsHead { get; set; }
        public string Passport { get; set; }
        public string Phone { get; set; }
        public int? PhotoId { get; set; }
        public DateTime? PhotoDate { get; set; }
        public bool CardIssued { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public byte[] RowId { get; set; }
        public short? Relationship { get; set; }
        public short? Profession { get; set; }
        public short? Education { get; set; }
        public string Email { get; set; }
        public bool? IsOffline { get; set; }
        public string TypeOfId { get; set; }
        public int? Hfid { get; set; }
        public string CurrentAddress { get; set; }
        public string GeoLocation { get; set; }
        public int? CurrentVillage { get; set; }
        public bool Vulnerability { get; set; }

        public TblEducations EducationNavigation { get; set; }
        public TblFamilies Family { get; set; }
        public TblGender GenderNavigation { get; set; }
        public TblHf Hf { get; set; }
        public TblPhotos Photo { get; set; }
        public TblProfessions ProfessionNavigation { get; set; }
        public TblRelations RelationshipNavigation { get; set; }
        public TblIdentificationTypes TypeOf { get; set; }
        public ICollection<TblClaim> TblClaim { get; set; }
        public ICollection<TblClaimDedRem> TblClaimDedRem { get; set; }
        public ICollection<TblFamilies> TblFamilies { get; set; }
        public ICollection<TblHealthStatus> TblHealthStatus { get; set; }
        public ICollection<TblInsureePolicy> TblInsureePolicy { get; set; }
        public ICollection<TblPolicyRenewalDetails> TblPolicyRenewalDetails { get; set; }
        public ICollection<TblPolicyRenewals> TblPolicyRenewals { get; set; }
    }
}
