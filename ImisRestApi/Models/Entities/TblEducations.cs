using System;
using System.Collections.Generic;

namespace ImisRestApi.Models.Entities
{
    public partial class TblEducations
    {
        public TblEducations()
        {
            TblInsuree = new HashSet<TblInsuree>();
        }

        public short EducationId { get; set; }
        public string Education { get; set; }
        public int? SortOrder { get; set; }
        public string AltLanguage { get; set; }

        public ICollection<TblInsuree> TblInsuree { get; set; }
    }
}
