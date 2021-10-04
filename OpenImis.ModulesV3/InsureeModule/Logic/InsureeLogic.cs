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

            response.PhotoBase64 = CreateBase64ImageFromFilepath(response.PhotoPath);

            return response;
        }

        public GetInsureeModel Get(string chfid)
        {
            GetInsureeModel response;

            response = insureeRepository.Get(chfid);

            return response;
        }

        public string CreateBase64ImageFromFilepath(string path)
        {
            var base64 = "";
            if (!String.IsNullOrEmpty(path))
            {
                var photoFolder = _configuration.GetValue<string>("AppSettings:UpdatedFolder");

                var startIndex = path.LastIndexOf("\\") ==  -1 ? 0 : path.LastIndexOf("\\");
                var fileName = path.Substring(startIndex);
                var fileFullPath = Path.Join(photoFolder, fileName);

                if (File.Exists(fileFullPath))
                {
                    base64 = Convert.ToBase64String(File.ReadAllBytes(fileFullPath));
                }
            }
            return base64;
        }
    }
}
