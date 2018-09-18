using System;
using System.Collections.Generic;

namespace OpenImis.DB.SqlServer
{
    public partial class TblGender
    {
        public TblGender()
        {
            TblInsuree = new HashSet<TblInsuree>();
        }

        public string Code { get; set; }
        public string Gender { get; set; }
        public string AltLanguage { get; set; }
        public int? SortOrder { get; set; }

        public ICollection<TblInsuree> TblInsuree { get; set; }
    }
}
