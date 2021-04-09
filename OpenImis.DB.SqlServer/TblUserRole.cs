using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.DB.SqlServer
{
    public class TblUserRole
    {
        public TblUserRole()
        {
        }

        public int UserRoleID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
    }
}
