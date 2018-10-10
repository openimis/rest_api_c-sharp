using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Responses
{
    public class SaveAckResponse : ImisApiResponse
    {
        public SaveAckResponse(Exception e) : base(e)
        {

        }
        public SaveAckResponse(int value, bool error) : base(value, error)
        {
            SetMessage(value);

        }

        public SaveAckResponse(int value, bool error, DataTable data) : base(value, error, data)
        {
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
                    msg.MessageValue = "1-Request for control number cannot be posted in the  external payment gateway";
                    Message = msg;
                    break;
            }
        }
    }
}
