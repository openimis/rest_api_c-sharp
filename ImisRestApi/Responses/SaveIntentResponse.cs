using ImisRestApi.Responses.Messages;
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
        public SaveIntentResponse(int value, bool error, int lang) : base(value, error,lang)
        {
            SetMessage(value);

        }

        public SaveIntentResponse(int value, bool error, DataTable data, int lang) : base(value, error, data,lang)
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
                    msg.MessageValue = new Language().GetMessage(language, "WrongFormatInsureeNo");
                    Message = msg;
                    break;
                case 2:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "InValidINmissingPC");
                    Message = msg;
                    break;
                case 3:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "InValidEOC");
                    Message = msg;
                    break;
                case 4:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "IncompatibleEO_PC");
                    Message = msg;
                    break;
                case 5:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "NoRenewalProduct");
                    Message = msg;
                    break;
                case 6:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "InsureeNoMissing");
                    Message = msg;
                    break;
                case 7:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "InsureeNotEnrolled");
                    Message = msg;
                    break;
                case 8:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "DuplicateCNAssigned");
                    Message = msg;
                    break;
                case 9:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "CantAssignCn2");
                    Message = msg;
                    break;
                case 10:
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "UnknownPaymentType");
                    Message = msg;
                    break;

            }
        }

    }
}
