using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Responses
{
    public class SavePayResponse:ImisApiResponse
    {
        public SavePayResponse(Exception e) : base(e)
        {

        }
        public SavePayResponse(int value, bool error) : base(value, error)
        {
            SetMessage(value);

        }

        public SavePayResponse(int value, bool error, DataTable data) : base(value, error, data)
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
                    msg.MessageValue = "1-Wrong or missing receiving date";
                    Message = msg;
                    break;
                case 2:
                    msg.Code = value;
                    msg.MessageValue = "2-Wrong format of input data";
                    Message = msg;
                    break;
                case 3:
                    msg.Code = value;
                    msg.MessageValue = "3-Wrong control_number";
                    Message = msg;
                    break;
                case 4:
                    msg.Code = value;
                    msg.MessageValue = "4-Wrong Amount";
                    Message = msg;
                    break;
                case 5:
                    msg.Code = value;
                    msg.MessageValue = "5-Duplicate Payment Amount";
                    Message = msg;
                    break;
            }
        }
    }
}
