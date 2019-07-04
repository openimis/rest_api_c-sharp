using Newtonsoft.Json;
using OpenImis.Modules.PaymentModule.Helpers.Messages;
using OpenImis.Modules.PaymentModule.Models;
using OpenImis.Modules.PaymentModule.Models.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace OpenImis.Modules.PaymentModule.Helpers
{
    public class RequestedCNResponse : ImisApiResponse
    {
        public RequestedCNResponse(Exception e) : base(e)
        {
        }

        public RequestedCNResponse(int value, bool error, int lang) : base(value, error, lang)
        {
            SetMessage(value);
        }

        public RequestedCNResponse(int value, bool error, DataTable data, int lang) : base(value, error, data, lang)
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
                    msg.MessageValue = new Messages.Language().GetMessage(language, "Success");
                    Message = msg;
                    break;
                case 1:
                    msg.Code = value;
                    msg.MessageValue = new Messages.Language().GetMessage(language, "WrongInternalIdFormat");
                    Message = msg;
                    break;
                case 2:
                    var jsonString = JsonConvert.SerializeObject(msg.Data);
                    var reqs = JsonConvert.DeserializeObject<List<AssignedControlNumber>>(jsonString).Select(x => x.internal_identifier).ToArray();
                    var Ids_string = string.Join(",", reqs);

                    msg.Code = value;
                    msg.MessageValue = new Messages.Language().GetMessage(language, "InvalidInternalId") + Ids_string;
                    Message = msg;
                    break;
            }
        }
    }
}
