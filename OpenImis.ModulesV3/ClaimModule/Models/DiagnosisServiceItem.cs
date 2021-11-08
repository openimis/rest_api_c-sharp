using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV3.ClaimModule.Models
{
    public class DiagnosisServiceItem
    {
        public List<CodeName> diagnoses { get; set; }
        public List<CodeNamePrice> services { get; set; }
        public List<CodeNamePrice> items { get; set; }
        public DateTime update_since_last { get; set; }
    }
}
