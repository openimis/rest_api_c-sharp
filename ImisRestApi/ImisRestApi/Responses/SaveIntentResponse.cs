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
                    msg.MessageValue = "Missing OfficerCode or Phonenumber";
                    Message = msg;
                    break;
                case 2:
                    msg.Code = value;
                    msg.MessageValue = "Invalid Officer Code";
                    Message = msg;
                    break;
                case 3:
                    msg.Code = value;
                    msg.MessageValue = "Empty InsuranceNumber";
                    Message = msg;
                    break;
                case 4:
                    msg.Code = value;
                    msg.MessageValue = "Missing product or Product does not exists";
                    Message = msg;
                    break;
                case 5:
                    msg.Code = value;
                    msg.MessageValue = "The family does't contain this product for renewal";
                    Message = msg;
                    break;
                case 6:
                    msg.Code = value;
                    msg.MessageValue = "Wrong match of Enrollment Officer agaists Product";
                    Message = msg;
                    break;
                case 7:
                    msg.Code = value;
                    msg.MessageValue = "Invalid Insuree/Product Match";
                    Message = msg;
                    break;
               
            }
        }

    }
}
