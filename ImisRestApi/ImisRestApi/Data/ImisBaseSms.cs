using ImisRestApi.Models.Sms;
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

namespace ImisRestApi.Data
{
    public class ImisBaseSms
    {
        private string SmsTampletes = string.Empty;

        public ImisBaseSms(IConfiguration config,IHostingEnvironment environment)
        {
            SmsTampletes = environment.ContentRootPath + @"\Chanels\Payment\Sms\Strings";
        }

        public virtual async Task<string> PushSMS(List<SmsContainer> containers)
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

            return response_message;

        }

        public virtual string GetMessage(string filename)
        {
            string text = File.ReadAllText(SmsTampletes + filename +".txt", Encoding.UTF8);
            return text;
        }
    }
}
