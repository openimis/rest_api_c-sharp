using ImisRestApi.Responses.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ImisRestApi.Responses
{
    public class GetCoverageResponse : ImisApiResponse
    {
        public GetCoverageResponse(Exception error):base(error)
        {

        }
        public GetCoverageResponse(int value,bool error,int lang) :base(value,error,lang)
        {
            SetMessage(value);
        }
        public GetCoverageResponse(int value,bool error,DataTable data, int lang) : base(value,error,data,lang)
        {
            var firstRow = data.Rows[0];
            var jsonString = JsonConvert.SerializeObject(data);
            var coverage_products = JsonConvert.DeserializeObject<List<CoverageProduct>>(jsonString);
            var _data = new { OtherNames = firstRow["OtherNames"], LastNames = firstRow["LastName"],BirthDate = firstRow["DOB"],CoverageProducts = coverage_products };
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

        private class CoverageProduct
        {
            public string ProductCode { get; set; }
            public string PolicyValue { get; set; }
            public string EffectiveDate { get; set; }
            public string ExpiryDate { get; set; }
            public string Status { get; set; }
            public string DedType { get; set; }
            [JsonProperty(PropertyName = "DeductionHospital")]
            public string Ded1 { get; set; }
            [JsonProperty(PropertyName = "DeductionNonHospital")]
            public string Ded2 { get; set; }
            [JsonProperty(PropertyName = "CeilingHospital")]
            public string Ceiling1 { get; set; }
            [JsonProperty(PropertyName = "CeilingNonHospital")]
            public string Ceiling2 { get; set; }
            public string AntenatalAmountLeft { get; set; }
            public string TotalVisitsLeft { get; set; }
            public string ConsultationAmountLeft { get; set; }
            public string DeliveryAmountLeft { get; set; }
            public string HospitalizationAmountLeft { get; set; }
            public string SurgeryAmountLeft { get; set; }
            public string TotalAdmissionsLeft { get; set; }
            public string TotalAntenatalLeft { get; set; }
            public string TotalConsultationsLeft { get; set; }        
        }
    }
}