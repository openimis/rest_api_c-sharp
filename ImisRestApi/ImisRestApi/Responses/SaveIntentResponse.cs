using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Responses
{
    public class SaveIntentResponse : ImisApiResponse
    {
        public SaveIntentResponse(Exception e) : base(e)
        {

        }
        public SaveIntentResponse(int value, bool error) : base(value, error)
        {
            SetMessage(value);

        }

        public SaveIntentResponse(int value, bool error, DataTable data) : base(value, error, data)
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
                    msg.MessageValue = "1-Wrong format of insurance number";
                    Message = msg;
                    break;
                case 2:
                    msg.Code = value;
                    msg.MessageValue = "2-Not valid insurance or missing product code";
                    Message = msg;
                    break;
                case 3:
                    msg.Code = value;
                    msg.MessageValue = "3-Not valid enrolment officer code";
                    Message = msg;
                    break;
                case 4:
                    msg.Code = value;
                    msg.MessageValue = "4-Enrolment officer code and insurance product code are not compatible";
                    Message = msg;
                    break;
                case 5:
                    msg.Code = value;
                    msg.MessageValue = "5-Beneficiary has no policy of specified insurance product for renewal";
                    Message = msg;
                    break;
                case 6:
                    msg.Code = value;
                    msg.MessageValue = "6-Missing insurance number";
                    Message = msg;
                    break;
                case 7:
                    msg.Code = value;
                    msg.MessageValue = "7-Control number cannot be assigned";
                    Message = msg;
                    break;
                case 8:
                    msg.Code = value;
                    msg.MessageValue = "8-Duplicated control number assigned";
                    Message = msg;
                    break;
            

            }
        }

    }
}
