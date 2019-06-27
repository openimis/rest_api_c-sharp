using OpenImis.Modules.InsureeModule.Helpers.Messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OpenImis.Modules.InsureeModule.Helpers
{
    public class GetCommissionResponse : ImisApiResponse
    {
        public GetCommissionResponse(Exception error) : base(error)
        {

        }
        public GetCommissionResponse(int value, bool error, int lang) : base(value, error, lang)
        {
            SetMessage(value);
        }

        public GetCommissionResponse(int value, bool error, object data, int lang) : base(value, error, data, lang)
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
