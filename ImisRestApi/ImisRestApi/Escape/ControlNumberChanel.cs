using ImisRestApi.Response;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ImisRestApi.Escape
{
    public static class ControlNumberChanel
    {

        
        public static ControlNumberRequest PostRequest(string Url, int PaymentId, float ExpectedAmount)
        {
            //Call Payment Getway
            //Request for Control Number Here
            ControlNumberRequest res = new ControlNumberRequest() { Code = HttpStatusCode.OK,RequestAcknowledged = false};
            return res;
        }

        public static void SendAcknowledgement()
        {

        }
    }
}
