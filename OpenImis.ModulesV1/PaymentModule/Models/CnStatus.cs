using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.PaymentModule.Models
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
