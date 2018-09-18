using System;
using System.Collections.Generic;

namespace OpenImis.DB.SqlServer
{
    public partial class TblPhotos
    {
        public TblPhotos()
        {
            TblInsuree = new HashSet<TblInsuree>();
        }

        public int PhotoId { get; set; }
        public int? InsureeId { get; set; }
        public string Chfid { get; set; }
        public string PhotoFolder { get; set; }
        public string PhotoFileName { get; set; }
        public int OfficerId { get; set; }
        public DateTime PhotoDate { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? AuditUserId { get; set; }
        public byte[] RowId { get; set; }

        public ICollection<TblInsuree> TblInsuree { get; set; }
    }
}
