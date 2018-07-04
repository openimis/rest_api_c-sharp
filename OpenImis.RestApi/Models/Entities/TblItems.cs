using System;
using System.Collections.Generic;

namespace OpenImis.RestApi.Models.Entities
{
    public partial class TblItems
    {
        public TblItems()
        {
            TblClaimItems = new HashSet<TblClaimItems>();
            TblPlitemsDetail = new HashSet<TblPlitemsDetail>();
            TblProductItems = new HashSet<TblProductItems>();
        }

        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public string ItemPackage { get; set; }
        public decimal ItemPrice { get; set; }
        public string ItemCareType { get; set; }
        public short? ItemFrequency { get; set; }
        public byte ItemPatCat { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public byte[] RowId { get; set; }

        public ICollection<TblClaimItems> TblClaimItems { get; set; }
        public ICollection<TblPlitemsDetail> TblPlitemsDetail { get; set; }
        public ICollection<TblProductItems> TblProductItems { get; set; }
    }
}
