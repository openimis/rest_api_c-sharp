using System;
using System.Collections.Generic;

namespace OpenImis.DB.SqlServer
{
    public partial class TblLanguages
    {
        public TblLanguages()
        {
            TblUsers = new HashSet<TblUsers>();
        }

        public string LanguageCode { get; set; }
        public string LanguageName { get; set; }
        public int? SortOrder { get; set; }

        public ICollection<TblUsers> TblUsers { get; set; }
    }
}
