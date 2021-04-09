using System;

namespace OpenImis.Modules.PolicyModule.Models
{
    public class DataMessage
    {
        public int Code { get; set; }
        public string MessageValue { get; set; }
        public bool ErrorOccured { get; set; }
        public object Data { get; set; }
    }
}
