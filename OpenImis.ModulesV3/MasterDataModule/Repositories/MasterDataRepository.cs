﻿using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.ModulesV3.MasterDataModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenImis.DB.SqlServer.DataHelper;

namespace OpenImis.ModulesV3.MasterDataModule.Repositories
{
    public class MasterDataRepository : IMasterDataRepository
    {
        private IConfiguration Configuration;

        public MasterDataRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<ConfirmationTypeModel> GetConfirmationTypes()
        {
            List<ConfirmationTypeModel> confirmationTypes;

            using (var imisContext = new ImisDB())
            {
                confirmationTypes = imisContext.TblConfirmationTypes
                    .Select(x => new ConfirmationTypeModel()
                    {
                        ConfirmationTypeCode = x.ConfirmationTypeCode,
                        ConfirmationType = x.ConfirmationType,
                        SortOrder = x.SortOrder.ToString(),
                        AltLanguage = x.AltLanguage
                    })
                    .ToList();
            }

            return confirmationTypes;
        }

        public List<ControlModel> GetControls()
        {
            List<ControlModel> controls;

            using (var imisContext = new ImisDB())
            {
                controls = imisContext.TblControls
                    .Select(x => new ControlModel()
                    {
                        FieldName = x.FieldName,
                        Adjustibility = x.Adjustibility
                    })
                    .ToList();
            }

            return controls;
        }

        public List<EducationLevelModel> GetEducations()
        {
            List<EducationLevelModel> educations;

            using (var imisContext = new ImisDB())
            {
                educations = imisContext.TblEducations
                    .Select(x => new EducationLevelModel()
                    {
                        EducationId = x.EducationId,
                        Education = x.Education,
                        SortOrder = x.SortOrder.ToString(),
                        AltLanguage = x.AltLanguage
                    })
                    .ToList();
            }

            return educations;
        }

        public List<FamilyTypeModel> GetFamilyTypes()
        {
            List<FamilyTypeModel> familyTypes;

            using (var imisContext = new ImisDB())
            {
                familyTypes = imisContext.TblFamilyTypes
                    .Select(x => new FamilyTypeModel()
                    {
                        FamilyTypeCode = x.FamilyTypeCode,
                        FamilyType = x.FamilyType,
                        SortOrder = x.SortOrder.ToString(),
                        AltLanguage = x.AltLanguage
                    })
                    .ToList();
            }

            return familyTypes;
        }

        public List<HFModel> GetHFs()
        {
            List<HFModel> hfs;

            using (var imisContext = new ImisDB())
            {
                hfs = imisContext.TblHf
                    .Where(h => h.ValidityTo == null)
                    .Select(x => new HFModel()
                    {
                        HFID = x.HfId,
                        HFCode = x.Hfcode,
                        HFName = x.Hfname,
                        LocationId = x.LocationId,
                        HFLevel = x.Hflevel
                    })
                    .ToList();
            }

            return hfs;
        }

        public List<IdentificationTypeModel> GetIdentificationTypes()
        {
            List<IdentificationTypeModel> identificationTypes;

            using (var imisContext = new ImisDB())
            {
                identificationTypes = imisContext.TblIdentificationTypes
                    .Select(x => new IdentificationTypeModel()
                    {
                        IdentificationCode = x.IdentificationCode,
                        IdentificationTypes = x.IdentificationTypes,
                        SortOrder = x.SortOrder.ToString(),
                        AltLanguage = x.AltLanguage
                    })
                    .ToList();
            }

            return identificationTypes;
        }

        public List<LanguageModel> GetLanguages()
        {
            List<LanguageModel> languages;

            using (var imisContext = new ImisDB())
            {
                languages = imisContext.TblLanguages
                    .Select(x => new LanguageModel()
                    {
                        LanguageCode = x.LanguageCode,
                        LanguageName = x.LanguageName,
                        SortOrder = x.SortOrder.ToString()
                    })
                    .ToList();
            }

            return languages;
        }

        public List<LocationModel> GetLocations()
        {
            List<LocationModel> locations;

            using (var imisContext = new ImisDB())
            {
                locations = imisContext.TblLocations
                    .Where(l => l.ValidityTo == null &&
                    !(l.LocationName == "Funding"
                        || l.LocationCode == "FR"
                        || l.LocationCode == "FD"
                        || l.LocationCode == "FW"
                        || l.LocationCode == "FV"
                    ))
                    .Select(x => new LocationModel()
                    {
                        LocationId = x.LocationId,
                        LocationCode = x.LocationCode,
                        LocationName = x.LocationName,
                        ParentLocationId = x.ParentLocationId,
                        LocationType = x.LocationType
                    })
                    .ToList();
            }

            return locations;
        }

        public List<OfficerModel> GetOfficers()
        {
            List<OfficerModel> officers;

            using (var imisContext = new ImisDB())
            {
                officers = imisContext.TblOfficer
                    .Where(o => o.ValidityTo == null)
                    .Select(x => new OfficerModel()
                    {
                        OfficerId = x.OfficerId,
                        OfficerUUID = x.OfficerUUID,
                        Code = x.Code,
                        LastName = x.LastName,
                        OtherNames = x.OtherNames,
                        Phone = x.Phone,
                        LocationId = x.LocationId,
                        OfficerIDSubst = x.OfficerIdsubst.ToString(),
                        WorksTo = x.WorksTo.GetValueOrDefault()
                    })
                    .ToList();
            }

            return officers;
        }

        public List<PayerModel> GetPayers()
        {
            List<PayerModel> payers;

            using (var imisContext = new ImisDB())
            {
                payers = imisContext.TblPayer
                    .Where(p => p.ValidityTo == null)
                    .Select(x => new PayerModel()
                    {
                        PayerId = x.PayerId,
                        PayerName = x.PayerName,
                        LocationId = x.LocationId
                    })
                    .ToList();
            }

            return payers;
        }

        public List<ProductModel> GetProducts()
        {
            List<ProductModel> products;

            using (var imisContext = new ImisDB())
            {
                products = imisContext.TblProduct
                    .Where(p => p.ValidityTo == null)
                    .Select(x => new ProductModel()
                    {
                        ProdId = x.ProdId,
                        ProductCode = x.ProductCode,
                        ProductName = x.ProductName,
                        LocationId = x.LocationId,
                        InsurancePeriod = x.InsurancePeriod,
                        DateFrom = x.DateFrom,
                        DateTo = x.DateTo,
                        ConversionProdId = x.ConversionProdId,
                        Lumpsum = x.LumpSum,
                        MemberCount = x.MemberCount,
                        PremiumAdult = x.PremiumAdult,
                        PremiumChild = x.PremiumChild,
                        RegistrationLumpsum = x.RegistrationLumpSum,
                        RegistrationFee = x.RegistrationFee,
                        GeneralAssemblyLumpSum = x.GeneralAssemblyLumpSum,
                        GeneralAssemblyFee = x.GeneralAssemblyFee,
                        StartCycle1 = x.StartCycle1,
                        StartCycle2 = x.StartCycle2,
                        StartCycle3 = x.StartCycle3,
                        StartCycle4 = x.StartCycle4,
                        GracePeriodRenewal = x.GracePeriodRenewal,
                        MaxInstallments = x.MaxInstallments,
                        WaitingPeriod = x.WaitingPeriod,
                        Threshold = x.Threshold,
                        RenewalDiscountPerc = x.RenewalDiscountPerc,
                        RenewalDiscountPeriod = x.RenewalDiscountPeriod,
                        AdministrationPeriod = x.AdministrationPeriod,
                        EnrolmentDiscountPerc = x.EnrolmentDiscountPerc,
                        EnrolmentDiscountPeriod = x.EnrolmentDiscountPeriod,
                        GracePeriod = x.GracePeriod
                    })
                    .ToList();
            }

            return products;
        }

        public List<ProfessionTypeModel> GetProfessions()
        {
            List<ProfessionTypeModel> professions;

            using (var imisContext = new ImisDB())
            {
                professions = imisContext.TblProfessions
                    .Select(x => new ProfessionTypeModel()
                    {
                        ProfessionId = x.ProfessionId,
                        Profession = x.Profession,
                        SortOrder = x.SortOrder.ToString(),
                        AltLanguage = x.AltLanguage
                    })
                    .ToList();
            }

            return professions;
        }

        public List<RelationTypeModel> GetRelations()
        {
            List<RelationTypeModel> relations;

            using (var imisContext = new ImisDB())
            {
                relations = imisContext.TblRelations
                    .Select(x => new RelationTypeModel()
                    {
                        RelationId = x.RelationId,
                        Relation = x.Relation,
                        SortOrder = x.SortOrder.ToString(),
                        AltLanguage = x.AltLanguage
                    })
                    .ToList();
            }

            return relations;
        }

        public List<PhoneDefaultModel> GetPhoneDefaults()
        {
            List<PhoneDefaultModel> phoneDefaults;

            using (var imisContext = new ImisDB())
            {
                phoneDefaults = imisContext.TblIMISDefaultsPhone
                    .Select(x => new PhoneDefaultModel()
                    {
                        RuleName = x.RuleName,
                        RuleValue = x.RuleValue
                    })
                    .ToList();
            }

            return phoneDefaults;
        }

        public List<GenderTypeModel> GetGenders()
        {
            List<GenderTypeModel> genders;

            using (var imisContext = new ImisDB())
            {
                genders = imisContext.TblGender
                    .Select(x => new GenderTypeModel()
                    {
                        Code = x.Code,
                        Gender = x.Gender,
                        AltLanguage = x.AltLanguage,
                        SortOrder = x.SortOrder.ToString()
                    })
                    .ToList();
            }

            return genders;
        }

        public List<MembershipGroupModel> GetMembershipGroup()
        {
            List<MembershipGroupModel> membershipGroups;

            using (var imisContext = new ImisDB())
            {
                membershipGroups = imisContext.TblMembershipGroup
                    .Select(x => new MembershipGroupModel()
                    {
                        idMembershipGroup = x.idMembershipGroup,
                        Name = x.Name
                    })
                    .ToList();
            }

            return membershipGroups;
        }

        public List<ServiceModel> GetServices()
        {
            List<ServiceModel> services;

            using (var imisContext = new ImisDB())
            {
                services = imisContext.TblServices
                    .Select(x => new ServiceModel()
                    {
                        ServiceId = x.ServiceId,
                        ServCode = x.ServCode,
                        ServName = x.ServName,
                        ServType = x.ServType,
                        ServLevel = x.ServLevel,
                        ServPrice = x.ServPrice,
                        ServCareType = x.ServCareType,
                        ServFrequency = x.ServFrequency,
                        ServPatCat = x.ServPatCat,
                        ValidityFrom = x.ValidityFrom,
                        ValidityTo = x.ValidityTo,
                        LegacyId = x.LegacyId,
                        ServCategory = x.ServCategory,
                        ServPackageType = x.ServPackageType
                    })
                    .ToList();
            }
            return services;
        }

        public List<ItemModel> GetItems()
        {
            List<ItemModel> items;

            using (var imisContext = new ImisDB())
            {
                items = imisContext.TblItems
                    .Select(x => new ItemModel()
                    {
                        ItemId = x.ItemId,
                        ItemCode = x.ItemCode,
                        ItemName = x.ItemName,
                        ItemType = x.ItemType,
                        ItemPackage = x.ItemPackage,
                        ItemPrice = x.ItemPrice,
                        ItemCareType = x.ItemCareType,
                        ItemFrequency = x.ItemFrequency,
                        ItemPatCat = x.ItemPatCat,
                        ValidityFrom = x.ValidityFrom,
                        ValidityTo = x.ValidityTo,
                        LegacyId = x.LegacyId
                    })
                    .ToList();
            }
            return items;
        }

        public List<SubServiceModel> GetSubServices()
        {
            List<SubServiceModel> subservices;

            using (var imisContext = new ImisDB())
            {
                subservices = imisContext.tblServiceContainedPackage
                    .Select(x => new SubServiceModel()
                    {
                        id = x.id,
                        ServiceId = x.ServiceId,
                        servicelinkedService = x.servicelinkedService,
                        qty_provided = x.qty_provided,
                        scpDate = x.scpDate,
                        price_asked = x.price_asked,
                        status = x.status
                    })
                    .ToList();
            }
            return subservices;
        }


        public List<SubItemModel> GetSubItems()
        {
            List<SubItemModel> subitems;

            using (var imisContext = new ImisDB())
            {
                subitems = imisContext.tblProductContainedPackage
                    .Select(x => new SubItemModel()
                    {
                        id = x.id,
                        ItemId = x.ItemId,
                        servicelinkedItem = x.servicelinkedItem,
                        qty_provided = x.qty_provided,
                        pcpDate = x.pcpDate,
                        price_asked = x.price_asked,
                        status = x.status
                    })
                    .ToList();
            }
            return subitems;
        }
    }
}