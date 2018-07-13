using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ImisRestApi.Responses
{
    public class DataMessage
    {
        
        public int Code { get; set; }
        public string MessageValue { get; set; }
        public bool ErrorOccured { get; set; }
        public object Data { get; set; }
    }
}