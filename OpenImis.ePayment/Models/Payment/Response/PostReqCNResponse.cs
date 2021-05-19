using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Models.Payment.Response
{
    public class PostReqCNResponse
    {
        public string ControlNumber { get; set; }
        public bool Posted { get; set; }
        public bool Assigned { get; set; }
        public int ErrorCode { get; set; }

        public bool ErrorOccured { get; set; }
        public string ErrorMessage { get; set; }
    }
}
