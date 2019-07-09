using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.DB.SqlServer
{
    public class TblRoleRight
    {
        public TblRoleRight()
        {
        }

        public int RoleRightID { get; set; }
        public int RoleID { get; set; }
        public int RightID { get; set; }
    }
}
