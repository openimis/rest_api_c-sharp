using System;
using System.Collections.Generic;

namespace OpenImis.DB.SqlServer
{
    public partial class TblHf
    {
        public TblHf()
        {
            TblClaim = new HashSet<TblClaim>();
            TblClaimAdmin = new HashSet<TblClaimAdmin>();
            TblHfcatchment = new HashSet<TblHfcatchment>();
            TblInsuree = new HashSet<TblInsuree>();
        }

        public int HfId { get; set; }
        public string Hfcode { get; set; }
        public string Hfname { get; set; }
        public string LegalForm { get; set; }
        public string Hflevel { get; set; }
        public string Hfsublevel { get; set; }
        public string Hfaddress { get; set; }
        public int LocationId { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string EMail { get; set; }
        public string HfcareType { get; set; }
        public int? PlserviceId { get; set; }
        public int? PlitemId { get; set; }
        public string AccCode { get; set; }
        public bool OffLine { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public byte[] RowId { get; set; }

        public TblHfsublevel HfsublevelNavigation { get; set; }
        public TblLegalForms LegalFormNavigation { get; set; }
        public TblLocations Location { get; set; }
        public TblPlitems Plitem { get; set; }
        public TblPlservices Plservice { get; set; }
        public ICollection<TblClaim> TblClaim { get; set; }
        public ICollection<TblClaimAdmin> TblClaimAdmin { get; set; }
        public ICollection<TblHfcatchment> TblHfcatchment { get; set; }
        public ICollection<TblInsuree> TblInsuree { get; set; }
    }
}
