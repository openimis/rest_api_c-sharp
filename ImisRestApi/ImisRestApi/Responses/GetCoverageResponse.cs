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
        public GetCoverageResponse(int value,bool error):base(value,error)
        {
            SetMessage(value);
        }
        public GetCoverageResponse(int value,bool error,DataTable data) : base(value,error,data)
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
                    msg.MessageValue = "Success.";
                    Message = msg;
                    break;
                case 1:

                    msg.Code = value;
                    msg.MessageValue = "Wrong Format or Missing Insurance Number of insuree";
                    Message = msg;
                    break;
                case 2:
                    msg.Code = value;
                    msg.MessageValue = "Insurance number of Insuree not found";
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
            public string Ded1 { get; set; }
            public string Ded2 { get; set; }
            public string Ceiling1 { get; set; }
            public string Ceiling2 { get; set; }
            public string AntenatalAmountLeft { get; set; }
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