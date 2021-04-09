using System;
using System.ComponentModel.DataAnnotations;

namespace OpenImis.Modules.ReportModule.Models
{
    public class ReportRequestModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
