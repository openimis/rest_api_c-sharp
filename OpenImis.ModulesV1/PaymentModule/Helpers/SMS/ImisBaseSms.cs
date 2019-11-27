using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenImis.ModulesV1.PaymentModule.Models;
using OpenImis.ModulesV1.PaymentModule.Models.SMS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenImis.ModulesV1.PaymentModule.Helpers.SMS
{
    public class ImisBaseSms
    {
        private string SmsTampletes = string.Empty;
        private string webRootPath;

        public ImisBaseSms(IConfiguration config, string webRootPath, string contentRootPath, Language language = Language.Primary)
        {
            this.webRootPath = webRootPath;

            if (language == Language.Primary)
                SmsTampletes = contentRootPath + @"\Escape\Sms\Strings\";
            else
                SmsTampletes = contentRootPath + @"\Escape\Sms\StringsSecondaryLanguage\";
        }

        public virtual async Task<string> SendSMS(List<SmsContainer> containers, string filename)
        {
            string response_message = string.Empty;


            HttpClient client = new HttpClient();

            var param = new { data = containers, datetime = DateTime.Now.ToString() };
            var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.ASCII, "application/json");

            try
            {
                var response = await client.PostAsync("url", content);
                var ret = await response.Content.ReadAsStringAsync();
                response_message = ret;
            }
            catch (Exception e)
            {
                response_message = e.ToString();
            }

            var msg = JsonConvert.SerializeObject(containers);
            SaveMessage(msg, filename);

            return response_message;

        }

        public virtual void SaveMessage(string message, string name)
        {
            //string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string mydocpath = System.IO.Path.Combine(webRootPath, "SentMessages");
            string namepart = new Random().Next(100000, 999999).ToString();

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(mydocpath, name + namepart + ".json")))
            {
                outputFile.WriteLine(message);
            }
        }

        public virtual string GetMessage(string filename)
        {
            string text = File.ReadAllText(SmsTampletes + filename + ".txt", Encoding.UTF8);
            return text;
        }

        public virtual async void QuickSms(string txtmsg, string phoneNumber, Language language = Language.Primary)
        {


            var txtmsgTemplate = string.Empty;
            string othersCount = string.Empty;

            List<SmsContainer> message = new List<SmsContainer>();
            message.Add(new SmsContainer() { Message = txtmsg, Recipient = phoneNumber });

            var fileName = "QuickSms_" + phoneNumber;

            string test = await SendSMS(message, fileName);
        }
    }
}
