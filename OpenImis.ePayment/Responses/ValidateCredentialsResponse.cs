using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenImis.ePayment.Responses
{
    public class ValidateCredentialsResponse
    {
        public bool ErrorOccured { get; set; }
        public bool success { get; set; }
    }
}