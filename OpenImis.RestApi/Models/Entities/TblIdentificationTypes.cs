using System;
using System.Collections.Generic;

namespace OpenImis.RestApi.Models.Entities
{
    public partial class TblIdentificationTypes
    {
        public TblIdentificationTypes()
        {
            TblInsuree = new HashSet<TblInsuree>();
        }

        public string IdentificationCode { get; set; }
        public string IdentificationTypes { get; set; }
        public string AltLanguage { get; set; }
        public int? SortOrder { get; set; }

        public ICollection<TblInsuree> TblInsuree { get; set; }
    }
}
