using System;
using System.Collections.Generic;

namespace OpenImis.DB.SqlServer
{
    public partial class TblInsureeAttachments
    {
        public TblInsureeAttachments()
        {
            // TblInsuree = new HashSet<TblInsuree>();
        }

        public int idAttachment { get; set; }
        public int? InsureeId { get; set; }
        public DateTime Date { get; set; }
        public string Folder { get; set; }
        public string Filename { get; set; }
        public string Title { get; set; }
        public string Mime { get; set; }
        public string Document { get; set; }

        // public ICollection<TblInsuree> TblInsuree { get; set; }
        public TblInsuree Insuree { get; set; }
    }
}
