using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models.Payment
{
    public class ControlNumberResp
    {
        [Required]
        public string PaymentId { get; set; }
        public string ControlNumber { get; set; }
        [Required]
        public bool ErrorOccured { get; set; }
        public string ErrorMessage { get; set; }
    }
}
