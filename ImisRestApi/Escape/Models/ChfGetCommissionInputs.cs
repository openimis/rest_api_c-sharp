using ImisRestApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Escape.Models
{
    public class ChfGetCommissionInputs
    {
        [Required]
        public string officer_code { get; set; }
        [Required]
        public string month { get; set; }
        [Required]
        public string year { get; set; }
        public int language { get; set; }
        public string product_code { get; set; }
        public string payer { get; set; }
        public string msisdn { get; set; }
    }
}
