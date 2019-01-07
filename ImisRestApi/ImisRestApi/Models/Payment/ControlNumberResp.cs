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
        public string InternalIdentifier { get; set; }
        public string ControlNumber { get; set; }
        public bool RequestAcknowledged { get; set; }
        [Required]
        public bool ErrorOccured { get; set; }
        public string ErrorMessage { get; set; }
    }
}
