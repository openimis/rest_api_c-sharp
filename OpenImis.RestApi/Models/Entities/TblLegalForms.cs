using System;
using System.Collections.Generic;

namespace OpenImis.RestApi.Models.Entities
{
    public partial class TblLegalForms
    {
        public TblLegalForms()
        {
            TblHf = new HashSet<TblHf>();
        }

        public string LegalFormCode { get; set; }
        public string LegalForms { get; set; }
        public int? SortOrder { get; set; }
        public string AltLanguage { get; set; }

        public ICollection<TblHf> TblHf { get; set; }
    }
}
