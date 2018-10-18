using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Responses
{
    public class MatchPayResponse : ImisApiResponse
    {
        public MatchPayResponse(Exception e) : base(e)
        {

        }
        public MatchPayResponse(int value, bool error) : base(value, error)
        {
            SetMessage(value);

        }

        public MatchPayResponse(int value, bool error, DataTable data) : base(value, error, data)
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
                    msg.MessageValue = "1-The paymentId does not exist";
                    Message = msg;
                    break;
            }
        }
    }
}
