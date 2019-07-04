using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.PaymentModule.Models
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
