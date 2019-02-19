using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace ImisRestApi.Responses
{
    internal class GetPaymentListResponse : ImisApiResponse
    {
        public GetPaymentListResponse(Exception e) : base(e)
        {

        }
        public GetPaymentListResponse(int value, bool error) : base(value, error)
        {
            SetMessage(value);

        }

        public GetPaymentListResponse(int value, bool error, DataTable data) : base(value, error, data)
        {
            var jsonString = JsonConvert.SerializeObject(data);
            var matched_payments = JsonConvert.DeserializeObject<List<PaymentLists>>(jsonString);
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
                    msg.MessageValue = "Success.";
                    Message = msg;
                    break;
                case 1:
                    msg.Code = value;
                    msg.MessageValue = "1-Date of last update wrong";
                    Message = msg;
                    break;
                case 2:
                    msg.Code = value;
                    msg.MessageValue = "1-Wrong code of claim administrator";
                    Message = msg;
                    break;
            }
        }
    }

    public class PaymentLists
    {
        public bool update_since_last { get; set; }
        public string health_facility_code { get; set; }
        public string health_facility_name { get; set; }
        public CodeNamePrice pricelist_services { get; set; }
        public CodeNamePrice pricelist_items { get; set; }
    }

    public class CodeNamePrice
    {
        public string code { get; set; }
        public string name { get; set; }
        public string price { get; set; }
    }
}