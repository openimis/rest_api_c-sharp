using Newtonsoft.Json;
using OpenImis.ModulesV1.CoverageModule.Helpers.Messages;
using OpenImis.ModulesV1.CoverageModule.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OpenImis.ModulesV1.CoverageModule.Helpers
{
    public class GetCoverageResponse : ImisApiResponse
    {
        public GetCoverageResponse(Exception error) : base(error)
        {

        }
        public GetCoverageResponse(int value, bool error, int lang) : base(value, error, lang)
        {
            SetMessage(value);
        }
        public GetCoverageResponse(int value, bool error, object data, int lang) : base(value, error, data, lang)
        {
            var jsonString = JsonConvert.SerializeObject(data);
            var _data = JsonConvert.DeserializeObject<CoverageModel>(jsonString);

            msg.Data = _data;
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
            }
        }
    }
}
