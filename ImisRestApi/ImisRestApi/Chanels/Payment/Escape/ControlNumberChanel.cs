using ImisRestApi.Chanels.Payment.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ImisRestApi.Chanels.Payment.Escape
{
    public static class ControlNumberChanel
    {
        public static ControlNumberRequest PostRequest()
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
