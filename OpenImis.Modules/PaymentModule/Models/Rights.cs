using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.PaymentModule.Models
{
    public enum Rights
    {
        PaymentSearch = 101401,
        PaymentAdd = 101402,
        PaymentEdit = 101403,
        PaymentDelete = 101404,

        InsureeSearch = 101101,
        InsureeAdd = 101102,
        InsureeEdit = 101103,
        InsureeDelete = 101104,
        InsureeEnquire = 101105,

        FindClaim = 111001,
        ClaimAdd = 111002,
    }
}
