using Newtonsoft.Json;
using OpenImis.ModulesV3.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV3.InsureeModule.Models
{
    public class DetailModel
    {
        public string ProductName { get; set; }
        [JsonConverter(typeof(IsoDateSerializer))]
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; }
        public float? DedType { get; set; }
        public decimal? Ded1 { get; set; }
        public decimal? Ded2 { get; set; }
        public decimal? Ceiling1 { get; set; }
        public decimal? Ceiling2 { get; set; }
        public string ProductCode { get; set; }

        public decimal? AntenatalAmountLeft { get; set; }
        public decimal? ConsultationAmountLeft { get; set; }
        public decimal? DeliveryAmountLeft { get; set; }
        public decimal? HospitalizationAmountLeft { get; set; }
        public decimal? SurgeryAmountLeft { get; set; }
        public decimal? TotalAdmissionsLeft { get; set; }
        public decimal? TotalAntenatalLeft { get; set; }
        public decimal? TotalConsultationsLeft { get; set; }
        public decimal? TotalDelivieriesLeft { get; set; }
        public decimal? TotalSurgeriesLeft { get; set; }
        public decimal? TotalVisitsLeft { get; set; }
        public decimal? PolicyValue { get; set; }
        [JsonConverter(typeof(IsoDateSerializer))]
        public DateTime EffectiveDate { get; set; }
    }
}
