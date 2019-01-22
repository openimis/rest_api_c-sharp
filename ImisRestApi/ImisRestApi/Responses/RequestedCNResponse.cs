using ImisRestApi.Models.Payment;
using ImisRestApi.Models.Payment.Response;
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
            var reqs = JsonConvert.DeserializeObject<List<AssignedControlNumber>>(jsonString);
            msg.Data = reqs;

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
                case 1:
                    msg.Code = value;
                    msg.MessageValue = "1-Wrong format of internal identifier";
                    Message = msg;
                    break;
                case 2:

                    var jsonString = JsonConvert.SerializeObject(msg.Data);
                    var reqs = JsonConvert.DeserializeObject<List<AssignedControlNumber>>(jsonString).Select(x => x.internal_identifier).ToArray();
                    var Ids_string = string.Join(",", reqs);

                    msg.Code = value;
                    msg.MessageValue = "2-Not valid internal identifier "+Ids_string;
                    Message = msg;
                    break;

            }
        }
    }
}
