﻿using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV3.MasterDataModule.Models;
using OpenImis.ModulesV3.MasterDataModule.Repositories;

namespace OpenImis.ModulesV3.MasterDataModule.Logic
{
    public class MasterDataLogic : IMasterDataLogic
    {
        private IConfiguration _configuration;
        protected IMasterDataRepository masterDataRepository;

        public MasterDataLogic(IConfiguration configuration)
        {
            _configuration = configuration;
            masterDataRepository = new MasterDataRepository(_configuration);
        }

        public MasterDataModel GetMasterData()
        {
            MasterDataModel masterdata = new MasterDataModel()
            {
                ConfirmationTypes = masterDataRepository.GetConfirmationTypes(),
                Controls = masterDataRepository.GetControls(),
                Education = masterDataRepository.GetEducations(),
                FamilyTypes = masterDataRepository.GetFamilyTypes(),
                HF = masterDataRepository.GetHFs(),
                IdentificationTypes = masterDataRepository.GetIdentificationTypes(),
                Languages = masterDataRepository.GetLanguages(),
                Locations = masterDataRepository.GetLocations(),
                Officers = masterDataRepository.GetOfficers(),
                Payers = masterDataRepository.GetPayers(),
                Products = masterDataRepository.GetProducts(),
                Professions = masterDataRepository.GetProfessions(),
                Relations = masterDataRepository.GetRelations(),
                PhoneDefaults = masterDataRepository.GetPhoneDefaults(),
                Genders = masterDataRepository.GetGenders(),
                MembershipGroup = masterDataRepository.GetMembershipGroup(),
                Services = masterDataRepository.GetServices(),
                Items = masterDataRepository.GetItems(),
                SubServices = masterDataRepository.GetSubServices(),
                SubItems = masterDataRepository.GetSubItems()
            };

            return masterdata;
        }
    }
}
