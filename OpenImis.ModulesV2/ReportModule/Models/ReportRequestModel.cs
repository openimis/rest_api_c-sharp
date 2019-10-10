using System;
using System.ComponentModel.DataAnnotations;

namespace OpenImis.ModulesV2.ReportModule.Models
{
    public class ReportRequestModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
