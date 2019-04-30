using System;
using System.Collections.Generic;

namespace OpenImis.DB.SqlServer
{
    public partial class TblUsers
    {
        public TblUsers()
        {
            TblClaim = new HashSet<TblClaim>();
            TblLogins = new HashSet<TblLogins>();
            TblUsersDistricts = new HashSet<TblUsersDistricts>();
        }

        public int UserId { get; set; }
        public string LanguageId { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string Phone { get; set; }
        public string LoginName { get; set; }
        public int RoleId { get; set; }
        public int? Hfid { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public byte[] Password { get; set; }
        public string DummyPwd { get; set; }
        public string EmailId { get; set; }
        public string PrivateKey { get; set; }
        public string StoredPassword { get; set; }

        public TblLanguages Language { get; set; }
        public ICollection<TblClaim> TblClaim { get; set; }
        public ICollection<TblLogins> TblLogins { get; set; }
        public ICollection<TblUsersDistricts> TblUsersDistricts { get; set; }
    }
}
