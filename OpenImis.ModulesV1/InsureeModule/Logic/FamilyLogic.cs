using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV1.InsureeModule.Models;
using OpenImis.ModulesV1.InsureeModule.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace OpenImis.ModulesV1.InsureeModule.Logic
{
    public class FamilyLogic : IFamilyLogic
    {
        private IConfiguration Configuration;
        protected IFamilyRepository familyRepository;

        public FamilyLogic(IConfiguration configuration)
        {
            Configuration = configuration;
            familyRepository = new FamilyRepository(Configuration);
        }

        public void SetUserId(int userId)
        {
            familyRepository.UserId = userId;
        }

        public List<FamilyModel> Get(string insureeNumber)
        {
            List<FamilyModel> response;

            response = familyRepository.Get(insureeNumber);

            return response;
        }

        public List<FamilyModelv2> GetMember(string insureeNumber, int order)
        {
            List<FamilyModelv2> response;

            response = familyRepository.GetMember(insureeNumber, order);

            return response;
        }

        public DataMessage AddNew(FamilyModelv3 model)
        {
            DataMessage message;

            message = familyRepository.AddNew(model);

            return message;
        }

        public DataMessage Edit(EditFamily model)
        {
            DataMessage message;

            message = familyRepository.Edit(model);

            return message;
        }

        public DataMessage AddMember(FamilyMember model)
        {
            DataMessage message;

            message = familyRepository.AddMember(model);

            return message;
        }

        public DataMessage EditMember(EditFamilyMember model)
        {
            DataMessage message;

            message = familyRepository.EditMember(model);

            return message;
        }

        public DataMessage DeleteMember(string insureeNumber)
        {
            DataMessage message;

            message = familyRepository.DeleteMember(insureeNumber);

            return message;
        }
    }
}
