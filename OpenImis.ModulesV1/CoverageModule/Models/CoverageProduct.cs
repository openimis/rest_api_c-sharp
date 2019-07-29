using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.CoverageModule.Models
{
    public class CoverageProduct
    {
        public string ProductCode { get; set; }
        public string PolicyValue { get; set; }
        public string EffectiveDate { get; set; }
        public string ExpiryDate { get; set; }
        public string Status { get; set; }
        public string DedType { get; set; }
        [JsonProperty(PropertyName = "DeductionHospital")]
        public string Ded1 { get; set; }
        [JsonProperty(PropertyName = "DeductionNonHospital")]
        public string Ded2 { get; set; }
        [JsonProperty(PropertyName = "CeilingHospital")]
        public string Ceiling1 { get; set; }
        [JsonProperty(PropertyName = "CeilingNonHospital")]
        public string Ceiling2 { get; set; }
        public string AntenatalAmountLeft { get; set; }
        public string TotalVisitsLeft { get; set; }
        public string ConsultationAmountLeft { get; set; }
        public string DeliveryAmountLeft { get; set; }
        public string HospitalizationAmountLeft { get; set; }
        public string SurgeryAmountLeft { get; set; }
        public string TotalAdmissionsLeft { get; set; }
        public string TotalAntenatalLeft { get; set; }
        public string TotalConsultationsLeft { get; set; }
    }
}
