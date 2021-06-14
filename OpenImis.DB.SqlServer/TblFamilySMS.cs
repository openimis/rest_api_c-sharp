using System;
using System.Collections.Generic;

namespace OpenImis.DB.SqlServer
{
    public partial class TblFamilySMS
    {
        public int FamilyId { get; set; }
        public bool ApprovalOfSMS { get; set; }
        public string LanguageOfSMS { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }

        public TblFamilies Family { get; set; }

    }
}
