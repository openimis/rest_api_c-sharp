using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ImisRestApi.Responses
{
    public class ImisApiResponse
    {
        public DataMessage msg = new DataMessage();

        public ImisApiResponse(Exception e)
        {
            msg.Code = -1;
            msg.ErrorOccured = true;
            msg.MessageValue = e.Message;
            Message = msg;
        }

        public ImisApiResponse(int value,bool error)
        {
            msg.Code = value;
            msg.ErrorOccured = error;
            Message = msg;

        }

        public ImisApiResponse(int value, bool error, DataTable data)
        {
            msg.Code = value;
            msg.ErrorOccured = error;
            msg.Data = data;
            Message = msg;
        }

        public DataMessage Message { get; set; }

    }
}