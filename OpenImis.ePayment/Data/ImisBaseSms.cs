using OpenImis.ePayment.Models;
using OpenImis.ePayment.Models.Sms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Data
{
    public class ImisBaseSms
    {
        private string SmsTampletes = string.Empty;
        private IHostingEnvironment env;

        public ImisBaseSms(IConfiguration config,IHostingEnvironment environment,Language language = Language.Primary)
        {
            env = environment;

            if(language == Language.Primary)
                SmsTampletes = environment.ContentRootPath + @"\Escape\Sms\Strings\";
            else
                SmsTampletes = environment.ContentRootPath + @"\Escape\Sms\StringsSecondaryLanguage\";
        }

        public virtual async Task<string> SendSMS(List<SmsContainer> containers,string filename)
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
            SaveMessage(msg,filename);

            return response_message;

        }

        public virtual void SaveMessage(string message,string name)
        {
            //Todo: replace with function create for ePayment payloads but in SentSMSs folder 
            string mydocpath = System.IO.Path.Combine(env.WebRootPath,"SentMessages");
            string namepart = new Random().Next(100000, 999999).ToString();

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(mydocpath, name +namepart+".json")))
            {
                    outputFile.WriteLine(message);
            }
        }

        public virtual string GetMessage(string filename)
        {
            string text = File.ReadAllText(SmsTampletes + filename +".txt", Encoding.UTF8);
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
