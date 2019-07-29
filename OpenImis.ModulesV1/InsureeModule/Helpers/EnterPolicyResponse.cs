using OpenImis.ModulesV1.InsureeModule.Helpers.Messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OpenImis.ModulesV1.InsureeModule.Helpers
{
    public class EnterPolicyResponse : ImisApiResponse
    {
        public EnterPolicyResponse(Exception e) : base(e)
        {

        }

        public EnterPolicyResponse(int value, bool error, int lang) : base(value, error, lang)
        {
            SetMessage(value);
        }
        public EnterPolicyResponse(int value, bool error, DataTable data, int lang) : base(value, error, data, lang)
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
                    msg.MessageValue = new Language().GetMessage(language, "WrongINMember");
                    Message = msg;
                    break;
                case 2:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "NotFountINMember");
                    Message = msg;
                    break;
                case 3:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongOrMissingPC");
                    Message = msg;
                    break;
                case 4:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongOrMissingEnrolDate");
                    Message = msg;
                    break;
                case 5:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "WrongOrMissingEOcode");
                    Message = msg;
                    break;
            }
        }
    }
}
