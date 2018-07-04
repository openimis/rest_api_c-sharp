using System;
using System.Collections.Generic;

namespace OpenImis.RestApi.Models.Entities
{
    public partial class TblFamilyTypes
    {
        public TblFamilyTypes()
        {
            TblFamilies = new HashSet<TblFamilies>();
        }

        public string FamilyTypeCode { get; set; }
        public string FamilyType { get; set; }
        public int? SortOrder { get; set; }
        public string AltLanguage { get; set; }

        public ICollection<TblFamilies> TblFamilies { get; set; }
    }
}
