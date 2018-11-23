using System;
using System.Collections.Generic;

namespace OpenImis.DB.SqlServer
{
    public partial class TblHfsublevel
    {
        public TblHfsublevel()
        {
            TblHf = new HashSet<TblHf>();
            TblProductSublevel1Navigation = new HashSet<TblProduct>();
            TblProductSublevel2Navigation = new HashSet<TblProduct>();
            TblProductSublevel3Navigation = new HashSet<TblProduct>();
            TblProductSublevel4Navigation = new HashSet<TblProduct>();
        }

        public string Hfsublevel { get; set; }
        public string HfsublevelDesc { get; set; }
        public int? SortOrder { get; set; }
        public string AltLanguage { get; set; }

        public ICollection<TblHf> TblHf { get; set; }
        public ICollection<TblProduct> TblProductSublevel1Navigation { get; set; }
        public ICollection<TblProduct> TblProductSublevel2Navigation { get; set; }
        public ICollection<TblProduct> TblProductSublevel3Navigation { get; set; }
        public ICollection<TblProduct> TblProductSublevel4Navigation { get; set; }
    }
}
