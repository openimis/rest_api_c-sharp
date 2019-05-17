using ImisRestApi.Responses.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Responses
{
    public class GetCommissionResponse:ImisApiResponse
    {
        public GetCommissionResponse(Exception error) : base(error)
        {

        }
        public GetCommissionResponse(int value, bool error, int lang) : base(value, error, lang)
        {
            SetMessage(value);
        }


        public GetCommissionResponse(int value, bool error, DataTable data, int lang) : base(value, error, data, lang)
        {
            msg.Data = data;
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
                    msg.MessageValue = new Language().GetMessage(language, "WrongOrMissingHeadIN");
                    Message = msg;
                    break;
                case 2:

                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "HeadINNotFound");
                    Message = msg;
                    break;
            }
        }
    }
}
