using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ePayment.Escape.Payment.Models
{
    abstract public class GePGValidatedResponseModel
    {
        public bool HasValidSignature { get; set; } 
    }
}
