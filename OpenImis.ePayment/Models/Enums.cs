using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Models
{
    public enum Language
    {
        Primary,
        Secondary
    }
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

    public static class Errors
    {
        public enum ControlNumbers
        {
            Success = 4001,
            NoControlNumberAvailable = 4002,
            ThresholdReached = 4003,
            UnexpectedException = 4999
        }
    }
}
