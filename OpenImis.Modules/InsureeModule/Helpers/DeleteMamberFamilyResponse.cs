using OpenImis.Modules.InsureeModule.Helpers.Messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OpenImis.Modules.InsureeModule.Helpers
{
    public class DeleteMamberFamilyResponse : ImisApiResponse
    {
        public DeleteMamberFamilyResponse(Exception e) : base(e) { }

        public DeleteMamberFamilyResponse(int value, bool error, int lang) : base(value, error, lang)
        {
            SetMessage(value);
        }
        public DeleteMamberFamilyResponse(int value, bool error, DataTable data, int lang) : base(value, error, data, lang)
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
                    msg.MessageValue = new Language().GetMessage(language, "WrongFormatMissingIN");
                    Message = msg;
                    break;
                case 2:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "INNotFount");
                    Message = msg;
                    break;
                case 3:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "MemberNotHead");
                    Message = msg;
                    break;
            }
        }
    }
}
