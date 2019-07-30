using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.PaymentModule.Models
{
    public class DataMessage
    {
        public int Code { get; set; }
        public string MessageValue { get; set; }
        public bool ErrorOccured { get; set; }
        public object Data { get; set; }
    }
}
