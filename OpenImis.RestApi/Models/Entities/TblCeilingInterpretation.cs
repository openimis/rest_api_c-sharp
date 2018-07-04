using System;
using System.Collections.Generic;

namespace OpenImis.RestApi.Models.Entities
{
    public partial class TblCeilingInterpretation
    {
        public TblCeilingInterpretation()
        {
            TblProduct = new HashSet<TblProduct>();
        }

        public string CeilingIntCode { get; set; }
        public string CeilingIntDesc { get; set; }
        public string AltLanguage { get; set; }
        public int? SortOrder { get; set; }

        public ICollection<TblProduct> TblProduct { get; set; }
    }
}
