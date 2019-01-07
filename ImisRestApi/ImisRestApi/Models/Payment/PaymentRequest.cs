using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models.Payment
{
    public class PaymentRequest
    {
        public List<Request> Requests { get; set; }
    }

    public class Request
    {
        public string InternalIdentifier { get; set; }
    }
}
