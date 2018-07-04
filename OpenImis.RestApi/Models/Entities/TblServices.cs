using System;
using System.Collections.Generic;

namespace OpenImis.RestApi.Models.Entities
{
    public partial class TblServices
    {
        public TblServices()
        {
            TblClaimServices = new HashSet<TblClaimServices>();
            TblPlservicesDetail = new HashSet<TblPlservicesDetail>();
            TblProductServices = new HashSet<TblProductServices>();
        }

        public int ServiceId { get; set; }
        public string ServCode { get; set; }
        public string ServName { get; set; }
        public string ServType { get; set; }
        public string ServLevel { get; set; }
        public decimal ServPrice { get; set; }
        public string ServCareType { get; set; }
        public short? ServFrequency { get; set; }
        public byte ServPatCat { get; set; }
        public DateTime? ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int? AuditUserId { get; set; }
        public byte[] RowId { get; set; }
        public string ServCategory { get; set; }

        public ICollection<TblClaimServices> TblClaimServices { get; set; }
        public ICollection<TblPlservicesDetail> TblPlservicesDetail { get; set; }
        public ICollection<TblProductServices> TblProductServices { get; set; }
    }
}
