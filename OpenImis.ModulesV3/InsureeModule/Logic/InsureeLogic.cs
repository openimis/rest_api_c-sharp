using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly ILoggerFactory _loggerFactory;

        public InsureeLogic(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _loggerFactory = loggerFactory;

            insureeRepository = new InsureeRepository(_configuration);
        }

        public GetEnquireModel GetEnquire(string chfid)
        {
            GetEnquireModel response;

            response = insureeRepository.GetEnquire(chfid);

            if (response != null && !string.IsNullOrEmpty(response.PhotoPath))
            {
                response.PhotoBase64 = PhotoUtils.CreateBase64ImageFromFilepath(
                    _configuration.GetValue<string>("AppSettings:UpdatedFolder"), 
                    response.PhotoPath,
                    _loggerFactory.CreateLogger<InsureeLogic>());
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
