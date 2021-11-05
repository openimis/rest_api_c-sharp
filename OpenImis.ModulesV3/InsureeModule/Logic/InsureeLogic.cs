using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV3.InsureeModule.Models;
using OpenImis.ModulesV3.InsureeModule.Repositories;
using OpenImis.ModulesV3.Utils;
using System;
using System.IO;

namespace OpenImis.ModulesV3.InsureeModule.Logic
{
    public class InsureeLogic : IInsureeLogic
    {
        private IConfiguration _configuration;
        protected IInsureeRepository insureeRepository;

        public InsureeLogic(IConfiguration configuration)
        {
            _configuration = configuration;

            insureeRepository = new InsureeRepository(_configuration);
        }

        public GetEnquireModel GetEnquire(string chfid)
        {
            GetEnquireModel response;

            response = insureeRepository.GetEnquire(chfid);

            if (response != null)
            {
                response.PhotoBase64 = PhotoUtils.CreateBase64ImageFromFilepath(_configuration.GetValue<string>("AppSettings:UpdatedFolder"), response.PhotoPath);
            }

            return response;
        }

        public GetInsureeModel Get(string chfid)
        {
            GetInsureeModel response;

            response = insureeRepository.Get(chfid);

            return response;
        }
    }
}
