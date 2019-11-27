using OpenImis.ModulesV1.InsureeModule.Helpers.Messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OpenImis.ModulesV1.InsureeModule.Helpers
{
    public class EditFamilyResponse : ImisApiResponse
    {
        public EditFamilyResponse(Exception e) : base(e)
        {

        }
        public EditFamilyResponse(int value, bool error, int lang) : base(value, error, lang)
        {
            SetMessage(value);

        }

        public EditFamilyResponse(int value, bool error, DataTable data, int lang) : base(value, error, data, lang)
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
                    msg.MessageValue = new Language().GetMessage(language, "WrongPVillageCode");
                    Message = msg;
                    break;
                case 4:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongCVillageCode");
                    Message = msg;
                    break;
                case 5:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongGender");
                    Message = msg;
                    break;
                case 6:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongConfirmationType");
                    Message = msg;
                    break;
                case 7:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongGroupType");
                    Message = msg;
                    break;
                case 8:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongMaritalStatus");
                    Message = msg;
                    break;
                case 9:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongEducation");
                    Message = msg;
                    break;
                case 10:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongProfession");
                    Message = msg;
                    break;
                case 11:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "FSPCodeNotFound");
                    Message = msg;
                    break;
                case 12:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongIdentificationType");
                    Message = msg;
                    break;
            }
        }


    }
}
