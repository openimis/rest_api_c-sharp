using OpenImis.ePayment.Responses.Messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace OpenImis.ePayment.Responses
{
    public class EditMemberFamilyResponse : ImisApiResponse
    {
        public EditMemberFamilyResponse(Exception e):base(e)
        {

        }
        public EditMemberFamilyResponse(int value,bool error, int lang) :base(value,error,lang)
        {
            SetMessage(value);
        }
        public EditMemberFamilyResponse(int value,bool error,DataTable data, int lang) :base(value,error,data,lang)
        {
            SetMessage(value);
        }

        private void SetMessage(int value)
        {
            switch (value)
            {
                case 0:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "Success");
                    Message = msg;
                    break;
                case 1:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongOrMissingHeadIN");
                    Message = msg;
                    break;
                case 2:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "HeadINNotFound"); 
                    Message = msg;
                    break;
                case 3:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongINMember"); 
                    Message = msg;
                    break;
                case 4:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "NotFountINMember"); 
                    Message = msg;
                    break;
                case 5:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongVillageCode"); 
                    Message = msg;
                    break;
                case 6:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongGender");
                    Message = msg;
                    break;
                case 7:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongMaritalStatus");
                    Message = msg;
                    break;
                case 8:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongEducation");
                    Message = msg;
                    break;
                case 9:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongProfession");
                    Message = msg;
                    break;
                case 10:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "FSPCodeNotFound");
                    Message = msg;
                    break;
                case 11:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongIdentificationType");
                    Message = msg;
                    break;
                case 12:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongRelationship"); 
                    Message = msg;
                    break;

            }

        }


    }
}