using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.InsureeModule.Models.EnrollFamilyModels
{
    public class Premium
    {
        public int PremiumId { get; set; }
        public int PolicyId { get; set; }
        public int PayerId { get; set; }
        public string Amount { get; set; }
        public string Receipt { get; set; }
        public DateTime? PayDate { get; set; }
        public string PayType { get; set; }
        public string isPhotoFee { get; set; }
        public string isOffline { get; set; }
    }
}
