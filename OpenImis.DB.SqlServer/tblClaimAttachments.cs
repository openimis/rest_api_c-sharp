using System;
using System.Collections.Generic;

namespace OpenImis.DB.SqlServer
{
    public partial class TblClaimAttachments
    {
        public TblClaimAttachments()
        {

        }

        public int? ClaimId { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string File { get; set; }
        public string Mime { get; set; }

        public TblClaim Claim { get; set; }
    }
}
