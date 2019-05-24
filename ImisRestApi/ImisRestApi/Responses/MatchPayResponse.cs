using ImisRestApi.Responses.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Responses
{
    public class MatchPayResponse : ImisApiResponse
    {
        public MatchPayResponse(Exception e) : base(e)
        {

        }
        public MatchPayResponse(int value, bool error, int lang) : base(value, error,lang)
        {
            SetMessage(value);

        }

        public MatchPayResponse(int value, bool error, DataTable data, int lang) : base(value, error, data,lang)
        {
            var jsonString = JsonConvert.SerializeObject(data);
            var matched_payments = JsonConvert.DeserializeObject<List<MatchedPayment>>(jsonString);
            var _data = matched_payments;
            msg.Data = _data;
        
            SetMessage(value);
        }

        private void SetMessage(int value)
        {
            switch (value)
            {
                case 0:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "Success");
                    Message = msg;
                    break;
                case 1:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "PayIdDoesntExist");
                    Message = msg;
                    break;
            }
        }
    }

    public class MatchedPayment {
        public string FdMsg { get; set; }
        public string ProductCode { get; set; }
        public string InsuranceNumber { get; set; }
        public string isActivated { get; set; }
        public int PaymentMatched { get; set; }
    }
}
