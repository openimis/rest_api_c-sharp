using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models
{
    public enum Rights
    {
        FindPayment = 101401,
        AddPayment = 101402,
        EditPayment = 101403,
        DeletePayment = 101404,

        FindFamily = 101001,
        AddFamily = 101002,
        EditFamily = 101003,
        DeleteFamily = 101004,
        ViewFamily = 101001,

        FindInsuree = 101101,
        AddInsuree = 101102,
        EditInsuree = 101103,
        DeleteInsuree = 101104,
        ViewInsuree = 101101,

        FindPolicy = 101201,
        AddPolicy = 101202,
        EditPolicy = 101203,
        DeletePolicy = 101204,
        RenewPolicy = 101205,
        ViewPolicy = 101201,

        FindContribution = 101301,
        AddContribution = 101302,
        EditContribution = 101303,
        DeleteContribution = 101304,
        ViewContribution = 101301,

        FindClaim = 111001,
        EnterClaim = 111002,
        EditClaim = 111003,
        DeleteClaim = 111004,
        LoadClaim = 111005,
        PrintClaim = 111006,
        SubmitClaim = 111007,
        ReviewClaim = 111008,
        UpdateClaims = 111010,
        ProcessClaims = 111011,
        ClaimOverview = 111014,

        BatchProcess = 111101,
        BatchFilter = 111102,
        BatchPreview = 111103,

        FindProduct = 121001,
        AddProduct = 121002,
        EditProduct = 121003,
        DeleteProduct = 121004,
        DuplicateProduct = 121005,
        ViewProduct = 121001,

        FindHealthFacility = 121101,
        AddHealthFacility = 121102,
        EditHealthFacility = 121103,
        DeleteHealthFacility = 121104,

        FindPriceListMedicalServices = 121201,
        AddPriceListMedicalServices = 121202,
        EditPriceListMedicalServices = 121203,
        DeletePriceListMedicalServices = 121204,
        DuplicatePriceListMedicalServices = 121205,

        FindPriceListMedicalItems = 121301,
        AddPriceListMedicalItems = 121302,
        EditPriceListMedicalItems = 121303,
        DeletePriceListMedicalItems = 121304,
        DuplicatePriceListMedicalItems = 121305,

        FindMedicalService = 121401,
        AddMedicalService = 121402,
        EditMedicalService = 121403,
        DeleteMedicalService = 121404,

        FindMedicalItem = 122101,
        AddMedicalItem = 122102,
        EditMedicalItem = 122103,
        DeleteMedicalItem = 122104,

        FindOfficer = 121501,
        AddOfficer = 121502,
        EditOfficer = 121503,
        DeleteOfficer = 121504,

        FindClaimAdministrator = 121601,
        AddClaimAdministrator = 121602,
        EditClaimAdministrator = 121603,
        DeleteClaimAdministrator = 121604,

        FindUser = 121701,
        AddUser = 121702,
        EditUser = 121703,
        DeleteUser = 121704,

        FindPayer = 121801,
        AddPayer = 121802,
        EditPayer = 121803,
        DeletePayer = 121804,
        ViewPayer = 121801,

        FindLocations = 121901,
        AddDistrict = 121902,
        EditDistrict = 121903,
        DeleteDistrict = 121904,


        FindUserProfile = 122001,
        AddUserProfile = 122002,
        DeleteUserProfile = 122004,
        EditUserProfile = 122003,

        DiagnosesUpload = 141001,
        DiagnosesDownload = 141002,

        HealthFacilitiesUpload = 1411001,
        HealthFacilitiesDownload = 141102,

        LocationUpload = 141201,
        LocationDonwload = 141202,

        MasterDataDownload = 151001,
        PhoneExtractsCreate = 151101,
        OfflineExtractCreate = 151201,
        ClaimXMLUpload = 151301,
        EnrolmentsUpload = 151401,
        FeedbackUpload = 151501,

        PrimaryOperationalIndicatorPolicies = 160001,
        PrimaryOperationalIndicatorsClaims = 160002,
        DerivedOperationalIndicators = 160003,
        ContributionCollection = 160004,
        ProductSales = 160005,
        ContributionDistribution = 160006,
        UserActivity = 160007,
        EnrolmentPerformanceIndicators = 160008,
        StatusOfRegister = 160009,
        InsureeWithoutPhotos = 160010,
        PaymentCategoryOverview = 160011,
        MatchingFunds = 160012,
        ClaimOverviewReport = 160013,
        PercentageReferrals = 160014,
        FamiliesInsureesOverview = 160015,
        PendingInsurees = 160016,
        Renewals = 160017,
        CapitationPayment = 160018,
        RejectedPhoto = 160019,
        ContributionPayment = 160020,
        ControlNumberAssignment = 160021,

       
        DatabaseBackup = 170001,
        DatabaseRestore = 170002,
        ExecuteScripts = 170003,
        EmailSettings = 170004,
        Utilities = 170005,

       
        AddFund = 181001,

      
        SelectClaimForReview = 111016,
        EnterFeedback = 111009,
        ValuateClaim = 111017,
        PremiumCollection = 160022,
        Reports = 160021,
        Indicators = 160023,
        PremiumDistribution = 160024,
        SelectClaimForFeedback = 111009,
        OverviewFamily = 101005,
        MovingInsuree = 101006,
      
        AddWard = 121902 ,
        EditWard = 121903,
        DeleteWard = 121904,

        AddVillage = 121902,
        EditVillage = 121903,
        DeleteVillage = 121904, 

    }
}
