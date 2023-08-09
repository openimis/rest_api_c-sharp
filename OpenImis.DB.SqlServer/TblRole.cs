using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenImis.DB.SqlServer
{
    public class TblRole
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string AltLanguage { get; set; }
        public int IsSystem { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int AuditUserId { get; set; }
        public int? LegacyId { get; set; }
        public Guid RoleUUID { get; set; }


        public ICollection<TblUserRole> TblUserRoles { get; set; }
        public ICollection<TblRoleRight> TblRoleRights { get; set; }
    }
}
