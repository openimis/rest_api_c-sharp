using OpenImis.ModulesV2.MasterDataModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.MasterDataModule.Repositories
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

    }
}
