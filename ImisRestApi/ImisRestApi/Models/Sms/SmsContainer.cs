using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models.Sms
{
    public class SmsContainer
    {
        public string Message { get; set; }
        public string Recipient { get; set; }
        public object Response { get; set; }
    }
}
