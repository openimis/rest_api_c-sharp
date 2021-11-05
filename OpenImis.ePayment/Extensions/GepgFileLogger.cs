using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace OpenImis.ePayment.Extensions
{
    public class GepgFileLogger
    {

        private int paymentId;
        private string finality;
        private string content;
        IHostingEnvironment env;
        string basePath;

        public GepgFileLogger(int paymentId, string finality, string content, IHostingEnvironment env)
        {
            this.paymentId = paymentId;
            this.finality = finality;
            this.content = content;
            this.env = env;
        }

        public GepgFileLogger(string finality, string content, string basePath)
        {
            this.finality = finality;
            this.content = content;
            this.basePath = basePath;
        }

        public static void Log(int paymentId, string finality, string content, IHostingEnvironment env)
        {
            Log(paymentId + "_" + finality, content, env);
        }

        public static void Log(string finality, string content, IHostingEnvironment env)
        {
            var currentDate = DateTime.Now.ToString("yyyy/M/d/");
            var currentDateTime = DateTime.Now.ToString("yyyy-M-dTHH-mm-ss");
            string targetPath = System.IO.Path.Combine(env.WebRootPath, "ePayment", currentDate);
            //if no Directory with current date - then create folder
            if (!Directory.Exists(targetPath))
            {
                System.IO.Directory.CreateDirectory(targetPath);
            }
            //we have target folder for current date - then we can save file
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(targetPath, finality + "_" + currentDateTime + ".json")))
            {
                outputFile.WriteLine(content);
            }
        }

        public void putRequestBody()
        {
            var currentDate = DateTime.Now.ToString("yyyy/M/d/");
            var currentDateTime = DateTime.Now.ToString("yyyy-M-dTHH-mm-ss");
            string targetPath = System.IO.Path.Combine(basePath, "ePayment", currentDate);
            //if no Directory with current date - then create folder
            if (!Directory.Exists(targetPath))
            {
                System.IO.Directory.CreateDirectory(targetPath);
            }
            //we have target folder for current date - then we can save file
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(targetPath, finality + "_" + currentDateTime + ".xml")))
            {
                outputFile.WriteLine(content);
            }
        }
    }
}
