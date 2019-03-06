using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models
{
    public enum Language
    {
        Primary,
        Secondary
    }
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
        MoveInsuree = 101105,

        FindPolicy = 101201,
        AddPolicy = 101202,
        EditPolicy = 101203,
        DeletePolicy = 101204,
        RenewPolicy = 101205,

        FindContribution = 101301,
        AddContribution = 101302,
        EditContribution = 101303,
        DeleteContribution = 101304,

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

        Tools = 130000,
        Registers = 131000,
        DiagnosesUpload = 131001, 
        DiagnosesDownload = 131002,

        HealthFacilitiesUpload = 131003,
        HealthFacilitiesDownload = 131004,

        LocationUpload = 131005,
        LocationDonwload = 131006,

        Extracts = 131100,
        MasterDataDownload = 131101,

        PhoneExtractsCreate = 131102,

        OfflineExtractCreate = 131103,

        ClaimXMLUpload = 131104,

        EnrolmentsUpload = 131105,
        FeedbackUpload = 131106,

        Reports = 131200,
        PrimaryOperationalIndicatorPolicies = 131201,
        PrimaryOperationalIndicatorsClaims = 131202,
        DerivedOperationalIndicators = 131203,
        ContributionCollection = 131204,
        ProductSales = 131205,
        ContributionDistribution = 131206,
        UserActivity = 131207,
        EnrolmentPerformanceIndicators = 131208,
        StatusOfRegister = 131209,
        InsureeWithoutPhotos = 131210,
        PaymentCategoryOverview = 131211,
        MatchingFunds = 131212,
        ClaimOverviewReport = 131213,
        PercentageReferrals = 131214,
        FamiliesInsureesOverview = 131215,
        PendingInsurees = 131216,
        Renewals = 131217,
        CapitationPayment = 131218,
        RejectedPhoto = 131219,
        ContributionPayment = 131220,
        ControlNumberAssignment = 131221, 
        OverviewOfCommissions = 131222,
        ClaimHistoryReport = 131223,
        Utilities = 131300,
        DatabaseBackup = 131301,
        DatabaseRestore = 131302,
        ExecuteScripts = 131303,
        EmailSettings = 131304,
        AddFund = 131401,


    }
}
