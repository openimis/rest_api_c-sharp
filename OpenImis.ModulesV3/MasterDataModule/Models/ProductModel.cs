using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV3.MasterDataModule.Models
{
    public class ProductModel
    {
        public int ProdId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int? LocationId { get; set; }
        public int InsurancePeriod { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public int? ConversionProdId { get; set; }
        public decimal Lumpsum { get; set; }
        public int MemberCount { get; set; }
        public decimal? PremiumAdult { get; set; }
        public decimal? PremiumChild { get; set; }
        public decimal? RegistrationLumpsum { get; set; }
        public decimal? RegistrationFee { get; set; }
        public decimal? GeneralAssemblyLumpSum { get; set; }
        public decimal? GeneralAssemblyFee { get; set; }
        public string StartCycle1 { get; set; }
        public string StartCycle2 { get; set; }
        public string StartCycle3 { get; set; }
        public string StartCycle4 { get; set; }
        public int? GracePeriodRenewal { get; set; }
        public int? MaxInstallments { get; set; }
        public int? WaitingPeriod { get; set; }
        public int? Threshold { get; set; }
        public int? RenewalDiscountPerc { get; set; }
        public int? RenewalDiscountPeriod { get; set; }
        public int? AdministrationPeriod { get; set; }
        public int? EnrolmentDiscountPerc { get; set; }
        public int? EnrolmentDiscountPeriod { get; set; }
        public int GracePeriod { get; set; }
    }
}