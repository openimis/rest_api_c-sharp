using System;
using System.Collections.Generic;

namespace OpenImis.RestApi.Models.Entities
{
    public partial class TblFromPhone
    {
        public int FromPhoneId { get; set; }
        public string DocType { get; set; }
        public string DocName { get; set; }
        public string DocStatus { get; set; }
        public DateTime LandedDate { get; set; }
        public string OfficerCode { get; set; }
        public string Chfid { get; set; }
        public DateTime? PhotoSumittedDate { get; set; }
        public int? ClaimId { get; set; }
    }
}
