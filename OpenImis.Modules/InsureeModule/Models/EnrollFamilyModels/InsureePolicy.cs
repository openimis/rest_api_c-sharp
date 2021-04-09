using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.InsureeModule.Models.EnrollFamilyModels
{
    public class InsureePolicy
    {
        public int InsureeId { get; set; }
        public int PolicyId { get; set; }
        public DateTime? EffectiveDate { get; set; }
    }
}
