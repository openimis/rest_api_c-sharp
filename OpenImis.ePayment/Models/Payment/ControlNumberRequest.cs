using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Response
{
    public class ControlNumberRequest
    {
        public HttpStatusCode Code { get; set; }
        public bool RequestAcknowledged { get; set; }
        public string ControlNumber { get; set; }
    }
}
