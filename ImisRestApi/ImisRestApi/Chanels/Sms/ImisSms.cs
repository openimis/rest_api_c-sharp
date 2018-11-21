using ImisRestApi.Data;
using ImisRestApi.Models.Sms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ImisRestApi.Chanels.Sms
{
    public class ImisSms:ImisBaseSms
    {
        private static string PRIVATE_KEY;
        private static string USER_ID;
        private static string URL;
        private static string SMS_RESOURCE;
        private static string REQUEST_TYPE;

        private static string sender = string.Empty;
        private static string service = string.Empty;
        private static Dictionary<string, string> configuredHeaders;

        public ImisSms(IConfiguration config, IHostingEnvironment env) :base(config,env)
        {
            PRIVATE_KEY = config["SmsGateWay:PrivateKey"];
            USER_ID = config["SmsGateWay:UserId"];
            URL = config["SmsGateWay:GateUrl"];
            SMS_RESOURCE = config["SmsGateWay:SmsResource"];
            REQUEST_TYPE = config["SmsGateWay:RequestType"];

            sender = config["SmsGateWay:SenderId"];
            service = config["SmsGateWay:ServiceId"];

            var headerKeys = config["SmsGateWay:HeaderKeys"].Split(",");
            var headerValues = config["SmsGateWay:HeaderValues"].Split(",");

            configuredHeaders = new Dictionary<string, string>();

            for (int i = 0;i < headerKeys.Count();i++)
            {
                configuredHeaders.Add(headerKeys[i], headerValues[i]);
            }

            //configuredHeaders = JsonConvert.DeserializeObject<Dictionary<string, string>>(headers);
        }

        public override async Task<string> SendSMS(List<SmsContainer> containers,string filename = null)
        {
            string response_message = string.Empty;

            foreach (var container in containers)
            {
                string message = container.Message;
                string recipients = container.Recipient;

                string json = GetRequestBody(message, recipients);

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);

                var headers = client.DefaultRequestHeaders;

                Headers _requiredheaders = new Headers(USER_ID, PRIVATE_KEY, json, REQUEST_TYPE);

                foreach (var _requiredheader in _requiredheaders.GetHeaders(configuredHeaders))
                {
                    headers.Add(_requiredheader.Key, _requiredheader.Value);
                }

                var param = new { data = json, datetime = DateTime.Now.ToString() };

                var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.ASCII, "application/json");

                try
                {
                    var response = await client.PostAsync(URL + SMS_RESOURCE, content);
                    var ret = await response.Content.ReadAsStringAsync();
                    response_message += ret;
                }
                catch (Exception e)
                {

                    response_message += e.ToString();
                }
            }

            return response_message;
        }


        private static string GetRequestBody(string message, string recipients)
        {

            string todayDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string body = "{\"message\":\"" + message + "\",\"datetime\":\"" + todayDate + "\",\"sender_id\":\"" + sender + "\",\"mobile_service_id\":\"" + service + "\",\"recipients\":\"" + recipients + "\"}";
            return body;
        }
    }
}
