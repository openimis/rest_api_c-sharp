using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenImis.ePayment.Extensions
{
    class GepgFileRequestLogger
    {
        private IHostingEnvironment _hostingEnvironment;
        private ILogger _logger;

        public GepgFileRequestLogger(IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = loggerFactory.CreateLogger<GepgFileRequestLogger>();
        }

        public void Log(string id, string content, string basePath, string extension)
        {
            try
            {
                var currentDate = DateTime.Now.ToString("yyyy/M/d/");
                string targetPath = Path.Combine(basePath, "ePayment", currentDate);
                var currentDateTime = DateTime.Now.ToString("yyyy-M-dTHH-mm-ss");

                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }

                using (StreamWriter outputFile = new StreamWriter(Path.Combine(targetPath,$"{id}_{currentDateTime}.{extension}")))
                {
                    outputFile.WriteLine(content);
                }
            } catch (Exception e)
            {
                _logger.LogError(e, $"Error while saving request details ({id})");
                _logger.LogDebug(content);
            }

        }

        public void Log(int paymentId, string finality, string content)
        {
            
            Log(paymentId + '_' + finality, content, _hostingEnvironment.WebRootPath, "json");
        }

        public void Log(string finality, string content)
        {
            Log(finality, content, _hostingEnvironment.WebRootPath, "json");
        }

        public void LogRequestBody(string finality, string content)
        {
            Log(finality, content, Path.Combine(Environment.CurrentDirectory, "wwwroot"), "xml");
        }





    }
}
