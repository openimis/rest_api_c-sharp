using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV2.InsureeModule.Models;
using OpenImis.ModulesV2.InsureeModule.Models.EnrollFamilyModels;
using OpenImis.ModulesV2.InsureeModule.Repositories;
using System;

namespace OpenImis.ModulesV2.InsureeModule.Logic
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

        public FamilyModel GetByCHFID(string chfid)
        {
            FamilyModel response;

            response = familyRepository.GetByCHFID(chfid);

            return response;
        }

        public int Create(EnrollFamilyModel model, int userId, int officerId)
        {
            int response;

            response = familyRepository.Create(model, userId, officerId);

            return response;
        }

        public int GetUserIdByUUID(Guid uuid)
        {
            int response;

            response = familyRepository.GetUserIdByUUID(uuid);

            return response;
        }

        public int GetOfficerIdByUserId(int userId)
        {
            int response;

            response = familyRepository.GetOfficerIdByUserId(userId);

            return response;
        }
    }
}
