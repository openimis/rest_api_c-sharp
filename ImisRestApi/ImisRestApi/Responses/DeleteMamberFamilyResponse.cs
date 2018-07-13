using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ImisRestApi.Responses
{
    public class DeleteMamberFamilyResponse : ImisApiResponse
    {
        public DeleteMamberFamilyResponse(Exception e) : base(e) { }

        public DeleteMamberFamilyResponse(int value, bool error):base(value,error)
        {
            SetMessage(value);
        }
        public DeleteMamberFamilyResponse(int value, bool error,DataTable data):base(value,error,data)
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
                    msg.MessageValue = "Wrong Format or Missing Insurance Number of Member";
                    Message = msg;
                    break;
                case 2:
                    msg.Code = value;
                    msg.MessageValue = "Insurance number of member not found";
                    Message = msg;
                    break;
                case 3:
                    msg.Code = value;
                    msg.MessageValue = "Mamber is head of family";
                    Message = msg;
                    break;
            }
        }
    }
}