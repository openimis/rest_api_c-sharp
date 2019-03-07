using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models
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

    }
}
