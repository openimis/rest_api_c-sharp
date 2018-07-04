using System;
using System.Collections.Generic;

namespace OpenImis.RestApi.Models.Entities
{
    public partial class TblPayer
    {
        public TblPayer()
        {
            TblPremium = new HashSet<TblPremium>();
        }

        public int PayerId { get; set; }
        public string PayerType { get; set; }
        public string PayerName { get; set; }
        public string PayerAddress { get; set; }
        public int? LocationId { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string EMail { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public byte[] RowId { get; set; }

        public TblLocations Location { get; set; }
        public TblPayerType PayerTypeNavigation { get; set; }
        public ICollection<TblPremium> TblPremium { get; set; }
    }
}
