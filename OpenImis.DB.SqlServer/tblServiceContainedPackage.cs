using System;
using System.Collections.Generic;

namespace OpenImis.DB.SqlServer
{
    public partial class tblServiceContainedPackage
    {
        public tblServiceContainedPackage()
        {

        }

        public int id { get; set; }
        public int ServiceId { get; set; }
        public int servicelinkedService { get; set; }
        public int qty_provided { get; set; }
        public DateTime scpDate { get; set; }
        public decimal price_asked { get; set; }
        public bool status { get; set; }

        public TblServices Service { get; set; }
    }
}
