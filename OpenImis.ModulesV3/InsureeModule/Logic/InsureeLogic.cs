using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV3.InsureeModule.Models;
using OpenImis.ModulesV3.InsureeModule.Repositories;
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

            if (!String.IsNullOrEmpty(response.PhotoPath))
            {
                var photoFolder = _configuration.GetValue<string>("AppSettings:UpdatedFolder");

                var startIndex = response.PhotoPath.LastIndexOf("\\");
                var fileName = response.PhotoPath.Substring(startIndex);
                var fileFullPath = Path.Join(photoFolder, fileName);

                if (File.Exists(fileFullPath))
                {
                    var base64 = Convert.ToBase64String(File.ReadAllBytes(fileFullPath));
                    response.PhotoBase64 = base64;
                }
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
