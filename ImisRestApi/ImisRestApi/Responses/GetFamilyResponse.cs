using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ImisRestApi.Responses
{
    public class GetFamilyResponse : ImisApiResponse
    {
        public GetFamilyResponse(Exception error) : base(error)
        {

        }
        public GetFamilyResponse(int value, bool error) : base(value, error)
        {
            SetMessage(value);
        }

 
        public GetFamilyResponse(int value, bool error, DataTable data) : base(value, error, data)
        {
            if (data.Rows.Count > 0)
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
                    msg.MessageValue = "Success.";
                    Message = msg;
                    break;
                case 1:

                    msg.Code = value;
                    msg.MessageValue = "Wrong Format or Missing Insurance Number of head";
                    Message = msg;
                    break;
                case 2:

                    msg.Code = value;
                    msg.MessageValue = "Insurance number of head not found";
                    Message = msg;
                    break;
            }
        }

    }
}