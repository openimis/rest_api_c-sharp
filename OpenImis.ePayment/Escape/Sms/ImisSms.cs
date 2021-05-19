using OpenImis.ePayment.Data;
using OpenImis.ePayment.Models;
using OpenImis.ePayment.Models.Sms;
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

namespace OpenImis.ePayment.Escape.Sms
{
    public class ImisSms:ImisBaseSms
    {

        IConfiguration config;

        public ImisSms(IConfiguration config, IHostingEnvironment env,Language lang = Language.Primary) : base(config, env,lang)
        {
            this.config = config;
        }

        public override async Task<string> SendSMS(List<SmsContainer> containers, string filename = null)
        {
            string response_message = string.Empty;

#if CHF
            string PRIVATE_KEY;
            string USER_ID;
            string URL;
            string SMS_RESOURCE;
            string REQUEST_TYPE;

            string sender = string.Empty;
            string service = string.Empty;
            Dictionary<string, string> configuredHeaders;


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

            for (int i = 0; i < headerKeys.Count(); i++)
            {
                configuredHeaders.Add(headerKeys[i], headerValues[i]);
            }

            //configuredHeaders = JsonConvert.DeserializeObject<Dictionary<string, string>>(headers);


            foreach (var container in containers)
            {
                string message = container.Message;
                string recipients = container.Recipient;
                
                string json = GetRequestBody(message, sender, service, recipients);

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

                var status = string.Empty;

                try
                {
                    var response = await client.PostAsync(URL + SMS_RESOURCE, content);
                    status = response.StatusCode.ToString();

                    var ret = await response.Content.ReadAsStringAsync();
                    response_message += ret;
                    container.Response = ret+"____"+ response.StatusCode;

                }
                catch (Exception e)
                {

                    response_message += e.ToString();
                    container.Response = status + " "+e.ToString();
                }
            }
#else
            // Generic approach without connection to an SMS Gateway 
            response_message = "Intent to send " + containers.Count + " SMSs";
#endif
            var msg = JsonConvert.SerializeObject(containers);
            await Task.Run(() => SaveMessage(msg, filename));

            return response_message;
        }


        private static string GetRequestBody(string message, string sender, string service, string recipients)
        {

            string todayDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string body = "{\"message\":\"" + message + "\",\"datetime\":\"" + todayDate + "\",\"sender_id\":\"" + sender + "\",\"mobile_service_id\":\"" + service + "\",\"recipients\":\"" + recipients + "\"}";
            return body;
        }
       
    }
}
