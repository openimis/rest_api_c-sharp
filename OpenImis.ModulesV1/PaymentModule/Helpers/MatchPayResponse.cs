using Newtonsoft.Json;
using OpenImis.ModulesV1.PaymentModule.Helpers.Messages;
using OpenImis.ModulesV1.PaymentModule.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OpenImis.ModulesV1.PaymentModule.Helpers
{
    public class MatchPayResponse : ImisApiResponse
    {
        public MatchPayResponse(Exception e) : base(e)
        {

        }
        public MatchPayResponse(int value, bool error, int lang) : base(value, error, lang)
        {
            SetMessage(value);

        }

        public MatchPayResponse(int value, bool error, DataTable data, int lang) : base(value, error, data, lang)
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
                    msg.MessageValue = new Messages.Language().GetMessage(language, "Success");
                    Message = msg;
                    break;
                case 1:
                    msg.Code = value;
                    msg.MessageValue = new Messages.Language().GetMessage(language, "PayIdDoesntExist");
                    Message = msg;
                    break;
            }
        }
    }
}
