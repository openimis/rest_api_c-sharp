using Microsoft.Extensions.Configuration;
using OpenImis.Modules.MasterDataModule.Models;
using OpenImis.Modules.MasterDataModule.Repositories;

namespace OpenImis.Modules.MasterDataModule.Logic
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
                Genders = masterDataRepository.GetGenders()
            };

            return masterdata;
        }
    }
}
