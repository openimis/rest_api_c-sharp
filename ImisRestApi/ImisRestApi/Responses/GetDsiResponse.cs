using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace ImisRestApi.Responses
{
    public class GetDsiResponse : ImisApiResponse
    {
 
        public GetDsiResponse(Exception e) : base(e)
        {

        }
        public GetDsiResponse(int value, bool error) : base(value, error)
        {
            SetMessage(value);

        }

        public GetDsiResponse(int value, bool error, DataTable data) : base(value, error, data)
        {
            var jsonString = JsonConvert.SerializeObject(data);
            var matched_payments = JsonConvert.DeserializeObject<List<DiagnosesServicesItems>>(jsonString);
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
            }
        }
    }

    public class DiagnosesServicesItems
    {
        public List<CodePrice> diagnoses { get; set; }
        public List<CodePrice> services { get; set; }
        public List<CodePrice> items { get; set; }
        public bool update_since_last { get; set; }
    }

    public class CodePrice
    {
        public string code { get; set; }
        public string price { get; set; }
    }

}