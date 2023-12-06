﻿using OpenImis.ModulesV3.MasterDataModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV3.MasterDataModule.Repositories
{
    public interface IMasterDataRepository
    {
        List<ConfirmationTypeModel> GetConfirmationTypes();
        List<ControlModel> GetControls();
        List<EducationLevelModel> GetEducations();
        List<FamilyTypeModel> GetFamilyTypes();
        List<HFModel> GetHFs();
        List<IdentificationTypeModel> GetIdentificationTypes();
        List<LanguageModel> GetLanguages();
        List<LocationModel> GetLocations();
        List<OfficerModel> GetOfficers();
        List<PayerModel> GetPayers();
        List<ProductModel> GetProducts();
        List<ProfessionTypeModel> GetProfessions();
        List<RelationTypeModel> GetRelations();
        List<PhoneDefaultModel> GetPhoneDefaults();
        List<GenderTypeModel> GetGenders();
        List<MembershipGroupModel> GetMembershipGroup();
        List<ServiceModel> GetServices();
        List<ItemModel> GetItems();
        List<SubServiceModel> GetSubServices();
        List<SubItemModel> GetSubItems();

    }
}
