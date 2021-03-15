using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models.Payment.Response
{
    public class AsignedControlNumbersResponse
    {
        public bool error_occured { get; set; }
        public List<AssignedControlNumber> assigned_control_numbers { get; set; }
    }
}
