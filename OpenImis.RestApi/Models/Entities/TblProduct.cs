using System;
using System.Collections.Generic;

namespace OpenImis.RestApi.Models.Entities
{
    public partial class TblProduct
    {
        public TblProduct()
        {
            InverseConversionProd = new HashSet<TblProduct>();
            TblClaimItems = new HashSet<TblClaimItems>();
            TblClaimServices = new HashSet<TblClaimServices>();
            TblPolicy = new HashSet<TblPolicy>();
            TblPolicyRenewals = new HashSet<TblPolicyRenewals>();
            TblProductItems = new HashSet<TblProductItems>();
            TblProductServices = new HashSet<TblProductServices>();
            TblRelDistr = new HashSet<TblRelDistr>();
            TblRelIndex = new HashSet<TblRelIndex>();
        }

        public int ProdId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int? LocationId { get; set; }
        public byte InsurancePeriod { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int? ConversionProdId { get; set; }
        public decimal LumpSum { get; set; }
        public short MemberCount { get; set; }
        public decimal? PremiumAdult { get; set; }
        public decimal? PremiumChild { get; set; }
        public decimal? DedInsuree { get; set; }
        public decimal? DedOpinsuree { get; set; }
        public decimal? DedIpinsuree { get; set; }
        public decimal? MaxInsuree { get; set; }
        public decimal? MaxOpinsuree { get; set; }
        public decimal? MaxIpinsuree { get; set; }
        public string PeriodRelPrices { get; set; }
        public string PeriodRelPricesOp { get; set; }
        public string PeriodRelPricesIp { get; set; }
        public string AccCodePremiums { get; set; }
        public string AccCodeRemuneration { get; set; }
        public decimal? DedTreatment { get; set; }
        public decimal? DedOptreatment { get; set; }
        public decimal? DedIptreatment { get; set; }
        public decimal? MaxTreatment { get; set; }
        public decimal? MaxOptreatment { get; set; }
        public decimal? MaxIptreatment { get; set; }
        public decimal? DedPolicy { get; set; }
        public decimal? DedOppolicy { get; set; }
        public decimal? DedIppolicy { get; set; }
        public decimal? MaxPolicy { get; set; }
        public decimal? MaxOppolicy { get; set; }
        public decimal? MaxIppolicy { get; set; }
        public int GracePeriod { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public byte[] RowId { get; set; }
        public decimal? RegistrationLumpSum { get; set; }
        public decimal? RegistrationFee { get; set; }
        public decimal? GeneralAssemblyLumpSum { get; set; }
        public decimal? GeneralAssemblyFee { get; set; }
        public string StartCycle1 { get; set; }
        public string StartCycle2 { get; set; }
        public int? MaxNoConsultation { get; set; }
        public int? MaxNoSurgery { get; set; }
        public int? MaxNoDelivery { get; set; }
        public int? MaxNoHospitalizaion { get; set; }
        public int? MaxNoVisits { get; set; }
        public decimal? MaxAmountConsultation { get; set; }
        public decimal? MaxAmountSurgery { get; set; }
        public decimal? MaxAmountDelivery { get; set; }
        public decimal? MaxAmountHospitalization { get; set; }
        public int? GracePeriodRenewal { get; set; }
        public int? MaxInstallments { get; set; }
        public int? WaitingPeriod { get; set; }
        public int? Threshold { get; set; }
        public int? RenewalDiscountPerc { get; set; }
        public int? RenewalDiscountPeriod { get; set; }
        public string StartCycle3 { get; set; }
        public string StartCycle4 { get; set; }
        public int? AdministrationPeriod { get; set; }
        public decimal? MaxPolicyExtraMember { get; set; }
        public decimal? MaxPolicyExtraMemberIp { get; set; }
        public decimal? MaxPolicyExtraMemberOp { get; set; }
        public decimal? MaxCeilingPolicy { get; set; }
        public decimal? MaxCeilingPolicyIp { get; set; }
        public decimal? MaxCeilingPolicyOp { get; set; }
        public int? EnrolmentDiscountPerc { get; set; }
        public int? EnrolmentDiscountPeriod { get; set; }
        public decimal? MaxAmountAntenatal { get; set; }
        public int? MaxNoAntenatal { get; set; }
        public string CeilingInterpretation { get; set; }
        public string Level1 { get; set; }
        public string Sublevel1 { get; set; }
        public string Level2 { get; set; }
        public string Sublevel2 { get; set; }
        public string Level3 { get; set; }
        public string Sublevel3 { get; set; }
        public string Level4 { get; set; }
        public string Sublevel4 { get; set; }
        public decimal? ShareContribution { get; set; }
        public decimal? WeightPopulation { get; set; }
        public decimal? WeightNumberFamilies { get; set; }
        public decimal? WeightInsuredPopulation { get; set; }
        public decimal? WeightNumberInsuredFamilies { get; set; }
        public decimal? WeightNumberVisits { get; set; }
        public decimal? WeightAdjustedAmount { get; set; }

        public TblCeilingInterpretation CeilingInterpretationNavigation { get; set; }
        public TblProduct ConversionProd { get; set; }
        public TblLocations Location { get; set; }
        public TblHfsublevel Sublevel1Navigation { get; set; }
        public TblHfsublevel Sublevel2Navigation { get; set; }
        public TblHfsublevel Sublevel3Navigation { get; set; }
        public TblHfsublevel Sublevel4Navigation { get; set; }
        public ICollection<TblProduct> InverseConversionProd { get; set; }
        public ICollection<TblClaimItems> TblClaimItems { get; set; }
        public ICollection<TblClaimServices> TblClaimServices { get; set; }
        public ICollection<TblPolicy> TblPolicy { get; set; }
        public ICollection<TblPolicyRenewals> TblPolicyRenewals { get; set; }
        public ICollection<TblProductItems> TblProductItems { get; set; }
        public ICollection<TblProductServices> TblProductServices { get; set; }
        public ICollection<TblRelDistr> TblRelDistr { get; set; }
        public ICollection<TblRelIndex> TblRelIndex { get; set; }
    }
}
