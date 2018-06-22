using System;
using System.Collections.Generic;

namespace ImisRestApi.Models.Entities
{
    public partial class TblProfessions
    {
        public TblProfessions()
        {
            TblInsuree = new HashSet<TblInsuree>();
        }

        public short ProfessionId { get; set; }
        public string Profession { get; set; }
        public int? SortOrder { get; set; }
        public string AltLanguage { get; set; }

        public ICollection<TblInsuree> TblInsuree { get; set; }
    }
}
