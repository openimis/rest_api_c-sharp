using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Chanels.Payment.Models
{
    public enum CnStatus
    {
        Sent,
        Acknowledged,
        Issued,
        Paid,
        Rejected
    }
}
