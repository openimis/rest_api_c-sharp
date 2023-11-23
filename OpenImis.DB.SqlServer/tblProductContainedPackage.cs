using System;
using System.Collections.Generic;

namespace OpenImis.DB.SqlServer
{
    public partial class tblProductContainedPackage
    {
        public tblProductContainedPackage()
        {

        }

        public int id { get; set; }
        public int ItemId { get; set; }
        public int servicelinkedItem { get; set; }
        public int qty_provided { get; set; }
        public DateTime pcpDate { get; set; }
        public decimal price_asked { get; set; }
        public bool status { get; set; }

        public TblItems Item { get; set; }
    }
}
