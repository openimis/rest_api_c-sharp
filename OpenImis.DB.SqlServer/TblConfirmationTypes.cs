using System;
using System.Collections.Generic;

namespace OpenImis.DB.SqlServer
{
    public partial class TblConfirmationTypes
    {
        public TblConfirmationTypes()
        {
            TblFamilies = new HashSet<TblFamilies>();
        }

        public string ConfirmationTypeCode { get; set; }
        public string ConfirmationType { get; set; }
        public int? SortOrder { get; set; }
        public string AltLanguage { get; set; }

        public ICollection<TblFamilies> TblFamilies { get; set; }
    }
}
