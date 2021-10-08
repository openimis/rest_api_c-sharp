using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV3.InsureeModule.Models;
using OpenImis.ModulesV3.InsureeModule.Models.EnrollFamilyModels;
using OpenImis.ModulesV3.InsureeModule.Repositories;
using OpenImis.ModulesV3.Utils;
using System;

namespace OpenImis.ModulesV3.InsureeModule.Logic
{
    public class FamilyLogic : IFamilyLogic
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        protected IFamilyRepository familyRepository;

        public FamilyLogic(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;

            familyRepository = new FamilyRepository(_configuration, _hostingEnvironment);
        }

        public FamilyModel GetByCHFID(string chfid, Guid userUUID)
        {
            FamilyModel response;

            response = familyRepository.GetByCHFID(chfid, userUUID);

            if (response != null)
            {
                foreach (var insure in response.Insurees)
                {
                    insure.PhotoBase64 = PhotoUtils.CreateBase64ImageFromFilepath(_configuration.GetValue<string>("AppSettings:UpdatedFolder"), insure.PhotoPath);
                }
            }

            return response;
        }

        public NewFamilyResponse Create(EnrollFamilyModel model, int userId, int officerId)
        {
            
            return familyRepository.Create(model, userId, officerId);

        }

        public int GetUserIdByUUID(Guid uuid)
        {
            int response;

            response = familyRepository.GetUserIdByUUID(uuid);

            return response;
        }

        public int GetOfficerIdByUserUUID(Guid userUUID)
        {
            int response;

            response = familyRepository.GetOfficerIdByUserUUID(userUUID);

            return response;
        }
    }
}
