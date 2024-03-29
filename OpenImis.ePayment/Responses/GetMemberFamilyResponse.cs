﻿using OpenImis.ePayment.Responses.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace OpenImis.ePayment.Responses
{
    public class GetMemberFamilyResponse : ImisApiResponse
    {
        public GetMemberFamilyResponse(Exception e):base(e)
        {

        }
        public GetMemberFamilyResponse(int value,bool error, int lang) :base(value,error,lang)
        {
            SetMessage(value);
        }
        public GetMemberFamilyResponse(int value, bool error,DataTable data, int lang) : base(value, error,data,lang)
        {
            if(data.Rows.Count > 0)
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
                case 3:                 
                    msg.Code = value;
                    msg.MessageValue = new Language().GetMessage(language, "NoMemberOfOrder");
                    Message = msg;
                    break;
            }
        }
    }
}