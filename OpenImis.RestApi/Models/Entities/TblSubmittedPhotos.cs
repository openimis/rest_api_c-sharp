using System;
using System.Collections.Generic;

namespace OpenImis.RestApi.Models.Entities
{
    public partial class TblSubmittedPhotos
    {
        public int PhotoId { get; set; }
        public string ImageName { get; set; }
        public string Chfid { get; set; }
        public string OfficerCode { get; set; }
        public DateTime? PhotoDate { get; set; }
        public DateTime? RegisterDate { get; set; }
    }
}
