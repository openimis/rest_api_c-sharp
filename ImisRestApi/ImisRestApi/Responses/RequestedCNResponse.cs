using ImisRestApi.Models.Payment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Responses
{
    public class RequestedCNResponse : ImisApiResponse
    {
        public RequestedCNResponse(Exception e) : base(e)
        {

        }
        public RequestedCNResponse(int value, bool error) : base(value, error)
        {
            SetMessage(value);

        }

        public RequestedCNResponse(int value, bool error, DataTable data) : base(value, error, data)
        {
            var jsonString = JsonConvert.SerializeObject(data);
            var reqs = JsonConvert.DeserializeObject<List<PaymentData>>(jsonString);
            msg.Data = reqs.Select(x => new { x.InternalIdentifier, x.ControlNumber });

            SetMessage(value);
        }

        private void SetMessage(int value)
        {
            switch (value)
            {
                case 0:
                    msg.Code = value;
                    msg.MessageValue = "Success.";
                    Message = msg;
                    break;
           
            }
        }
    }
}
