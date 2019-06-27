using OpenImis.Modules.InsureeModule.Helpers.Messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OpenImis.Modules.InsureeModule.Helpers
{
    public class EnterMemberFamilyResponse : ImisApiResponse
    {
        public EnterMemberFamilyResponse(Exception e) : base(e)
        {

        }
        public EnterMemberFamilyResponse(int value, bool error, int lang) : base(value, error, lang)
        {
            SetMessage(value);
        }
        public EnterMemberFamilyResponse(int value, bool error, DataTable data, int lang) : base(value, error, data, lang)
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
                    msg.MessageValue = new Language().GetMessage(language, "WrongOrMissingGender");
                    Message = msg;
                    break;
                case 5:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongFrOrMissingBd");
                    Message = msg;
                    break;
                case 6:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "MissingLastName");
                    Message = msg;
                    break;
                case 7:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "MissingOtherName");
                    Message = msg;
                    break;
                case 8:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "DuplicatedMemberIN");
                    Message = msg;
                    break;
                case 9:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongVillageCode");
                    Message = msg;
                    break;
                case 10:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongMaritalStatus");
                    Message = msg;
                    break;
                case 11:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongEducation");
                    Message = msg;
                    break;
                case 12:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongProfession");
                    Message = msg;
                    break;
                case 13:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "FSPCodeNotFound");
                    Message = msg;
                    break;
                case 14:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongRelationship");
                    Message = msg;
                    break;
                case 15:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongIdentificationType");
                    Message = msg;
                    break;
            }
        }
    }
}
