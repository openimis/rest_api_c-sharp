using Newtonsoft.Json;
using OpenImis.Modules.PolicyModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.Helpers
{
    public class ImisApiResponse
    {
        public DataMessage msg = new DataMessage();
        public int language { get; set; }

        public ImisApiResponse(Exception e)
        {
            msg.Code = -1;
            msg.ErrorOccured = true;
            msg.MessageValue = e.Message;
            Message = msg;
        }

        public ImisApiResponse(int value, bool error, int lang)
        {
            if (value != 0)
                error = true;

            language = lang;

            msg.Code = value;
            msg.ErrorOccured = error;
            Message = msg;
        }

        public ImisApiResponse(int value, bool error, object data, int lang)
        {
            if (value != 0)
                error = true;

            language = lang;

            msg.Code = value;
            msg.ErrorOccured = error;
            msg.Data = JsonConvert.SerializeObject(data);
            Message = msg;
        }

        public DataMessage Message { get; set; }
    }
}
