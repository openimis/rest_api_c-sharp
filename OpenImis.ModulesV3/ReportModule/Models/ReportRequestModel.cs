using System;
using System.ComponentModel.DataAnnotations;

namespace OpenImis.ModulesV3.ReportModule.Models
{
    public class ReportRequestModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
