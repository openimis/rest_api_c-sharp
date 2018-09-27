using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Responses
{
    public class CtrlNumberResponse:ImisApiResponse
    {
        public CtrlNumberResponse(Exception e) : base(e)
        {

        }
        public CtrlNumberResponse(int value, bool error) : base(value, error)
        {
            SetMessage(value);

        }

        public CtrlNumberResponse(int value, bool error, DataTable data) : base(value, error, data)
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
                    msg.MessageValue = "1-Control number cannot be assigned by the external payment gateway ";
                    Message = msg;
                    break;
            }
        }
    }
}
