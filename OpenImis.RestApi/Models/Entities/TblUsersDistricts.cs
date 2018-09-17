using System;
using System.Collections.Generic;

namespace OpenImis.RestApi.Models.Entities
{
    public partial class TblUsersDistricts
    {
        public int UserDistrictId { get; set; }
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }

        public TblLocations Location { get; set; }
        public TblUsers TblUsers { get; set; }
    }
}
