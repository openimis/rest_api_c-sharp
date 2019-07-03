using Newtonsoft.Json;
using OpenImis.Modules.InsureeModule.Helpers.Messages;
using OpenImis.Modules.InsureeModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenImis.Modules.InsureeModule.Helpers
{
    public class GetFamilyResponse : ImisApiResponse
    {
        public GetFamilyResponse(Exception error) : base(error)
        {
        }

        public GetFamilyResponse(int value, bool error, int lang) : base(value, error, lang)
        {
            SetMessage(value);
        }

        public GetFamilyResponse(int value, bool error, List<FamilyModel> data, int lang) : base(value, error, data, lang)
        {
            if (data.Count > 0)
            {
                var jsonString = JsonConvert.SerializeObject(data);
                var ObjectList = JsonConvert.DeserializeObject<List<object>>(jsonString);
                msg.Data = ObjectList.FirstOrDefault();
            }
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
            }
        }
    }
}
