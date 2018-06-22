using System;
using System.Collections.Generic;

namespace ImisRestApi.Models.Entities
{
    public partial class TblRelations
    {
        public TblRelations()
        {
            TblInsuree = new HashSet<TblInsuree>();
        }

        public short RelationId { get; set; }
        public string Relation { get; set; }
        public int? SortOrder { get; set; }
        public string AltLanguage { get; set; }

        public ICollection<TblInsuree> TblInsuree { get; set; }
    }
}
