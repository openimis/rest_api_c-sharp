using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ImisRestApi.Responses
{
    public class EditMemberFamilyResponse : ImisApiResponse
    {
        public EditMemberFamilyResponse(Exception e):base(e)
        {

        }
        public EditMemberFamilyResponse(int value,bool error):base(value,error)
        {
            SetMessage(value);
        }
        public EditMemberFamilyResponse(int value,bool error,DataTable data):base(value,error,data)
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
                    msg.MessageValue = "Wrong Format or Missing Insurance Number of head.";
                    Message = msg;
                    break;
                case 2:
                    msg.Code = value;
                    msg.MessageValue = "Insurance number of head not found";
                    Message = msg;
                    break;
                case 3:
                    msg.Code = value;
                    msg.MessageValue = "Wrong format of insurance number of member";
                    Message = msg;
                    break;
                case 4:
                    msg.Code = value;
                    msg.MessageValue = "Insurance number of member not found";
                    Message = msg;
                    break;
                case 5:
                    msg.Code = value;
                    msg.MessageValue = "Wrong current village code";
                    Message = msg;
                    break;
                case 6:
                    msg.Code = value;
                    msg.MessageValue = "Wrong gender";
                    Message = msg;
                    break;
                case 7:
                    msg.Code = value;
                    msg.MessageValue = "Wrong marital status";
                    Message = msg;
                    break;
                case 8:
                    msg.Code = value;
                    msg.MessageValue = "Wrong education";
                    Message = msg;
                    break;
                case 9:
                    msg.Code = value;
                    msg.MessageValue = "Wrong profession";
                    Message = msg;
                    break;
                case 10:
                    msg.Code = value;
                    msg.MessageValue = "FSP code not found";
                    Message = msg;
                    break;

            }

        }


    }
}