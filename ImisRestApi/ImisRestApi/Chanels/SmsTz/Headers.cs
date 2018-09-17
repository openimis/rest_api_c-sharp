using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ImisRestApi.Chanels.Sms
{
    public class Headers
    {
        private Dictionary<string, string> OutputHeaders = new Dictionary<string, string>();
        private string _userId;
        private string _messageBody;
        private string _privateKey;
        private string _requestType;

        public Headers(string userId,string privateKey, string messageBody, string requestType)
        {
            _userId = userId;
            _messageBody = messageBody;
            _privateKey = privateKey;
            _requestType = requestType;
        }
        public Dictionary<string, string> GetHeaders(Dictionary<string,string> requiredheaders) {

            foreach (var requiredheader in requiredheaders)
            {
                Type thisType = this.GetType();
                MethodInfo _method = thisType.GetMethod(requiredheader.Value);
                var headerVal =_method.Invoke(this,null);

                OutputHeaders.Add(requiredheader.Key, headerVal.ToString());
            }

            return OutputHeaders;
        }

        private string UserId()
        {
            return _userId;
        }

        private string HashMessage1()
        {
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyBytes = encoding.GetBytes(_privateKey);
            byte[] messageBytes = encoding.GetBytes(_messageBody);

            byte[] hashBytes = new HMACSHA256(keyBytes).ComputeHash(messageBytes);
            string hashmessage = Convert.ToBase64String(hashBytes);

            return hashmessage;
        }

        private string RequestType() {
            return _requestType;
        }
    }
}
