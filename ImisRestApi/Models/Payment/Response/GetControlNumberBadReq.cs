using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models.Payment.Response
{
    public class GetControlNumberBadReq
    {
        public bool error_occured { get; set; }
        public string error_message { get; set; }
    }
}
