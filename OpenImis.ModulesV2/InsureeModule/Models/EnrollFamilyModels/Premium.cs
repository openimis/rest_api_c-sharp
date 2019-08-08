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
        public DateTime PayDate { get; set; }
        public int PayType { get; set; }
        public bool isPhotoFee { get; set; }
        public bool isOffline { get; set; }
    }
}
