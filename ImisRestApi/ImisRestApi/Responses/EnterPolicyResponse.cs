using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ImisRestApi.Responses
{
    public class EnterPolicyResponse:ImisApiResponse
    {
        public EnterPolicyResponse(Exception e):base(e)
        {

        }

        public EnterPolicyResponse(int value, bool error):base(value,error)
        {
            SetMessage(value);
        }
        public EnterPolicyResponse(int value,bool error,DataTable data):base(value,error,data)
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
                    msg.MessageValue = "Wrong Format or Missing Insurance Number";
                    Message = msg;
                    break;
                case 2:
                    msg.Code = value;
                    msg.MessageValue = "Insurance number not found";
                    Message = msg;
                    break;
                case 3:
                    msg.Code = value;
                    msg.MessageValue = "Wrong or missing product code (not existing or not applicable to the family/group)";
                    Message = msg;
                    break;
                case 4:
                    msg.Code = value;
                    msg.MessageValue = "Wrong or missing enrolment date";
                    Message = msg;
                    break;
                case 5:
                    msg.Code = value;
                    msg.MessageValue = "Wrong or missing enrolment officer code (not existing or not applicable to the family/group)";
                    Message = msg;
                    break;

            }
        }
    }
}