using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using ImisRestApi.Models;
using Microsoft.Extensions.Configuration;

namespace ImisRestApi.Repo
{
    public class PaymentRepo : Connection
    {
        public int PaymentId { get; set; }
        private IntentOfPay _intent { get; set; }

        public PaymentRepo(IConfiguration configuration) :base(configuration)
        {

        }

        public PaymentRepo(IConfiguration configuration,IntentOfPay Intent) : base(configuration)
        {
            _intent = Intent;
        }

        public void SaveIntent()
        {
            XElement PaymentIntent = new XElement("Payment",
                new XElement("PhoneNumber", _intent.PhoneNumber),
                new XElement("RequestDate", _intent.Request_Date),
                new XElement("EnrolmentOfficerCode", _intent.EnrolmentOfficerCode),
                new XElement("PaymentDetails",
                              from d in _intent.PaymentDetails
                              select
                              new XElement("Payment",
                                  new XElement("InsureeNumber", d.InsureeNumber),
                                  new XElement("ProductCode", d.ProductCode)
                                  )
                              ));

            SqlParameter[] sqlParameters = {
                new SqlParameter("@Xml", PaymentIntent),
            };
            var data = Procedure("uspInsertPaymentIntent", sqlParameters);
        }

        public bool Valid(string InsureeNumber,string ProductCode)
        {
            return false;
        }
    }
}
