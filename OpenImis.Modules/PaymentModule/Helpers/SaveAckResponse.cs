using OpenImis.Modules.PaymentModule.Helpers.Messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OpenImis.Modules.PaymentModule.Helpers
{
    public class SaveAckResponse : ImisApiResponse
    {
        public SaveAckResponse(Exception e) : base(e)
        {

        }

        public SaveAckResponse(int value, bool error, int lang) : base(value, error, lang)
        {
            SetMessage(value);
        }

        public SaveAckResponse(int value, bool error, DataTable data, int lang) : base(value, error, data, lang)
        {
            SetMessage(value);
        }

        private void SetMessage(int value)
        {
            switch (value)
            {
                case 0:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "Success"); ;
                    Message = msg;
                    break;
                case 1:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "CantPostReq");
                    Message = msg;
                    break;
            }
        }
    }
}
