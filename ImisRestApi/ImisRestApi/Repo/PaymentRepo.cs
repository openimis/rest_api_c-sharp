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
            XElement PaymentIntent = new XElement("PaymentIntent",
                new XElement("Header", 
                    new XElement("OfficerCode", _intent.EnrolmentOfficerCode),
                    new XElement("RequestDate", _intent.Request_Date),
                    new XElement("PhoneNumber", _intent.PhoneNumber),
                    new XElement("AuditUserId", -1)
                ),         
                new XElement("Details",
                              from d in _intent.PaymentDetails
                              select
                              new XElement("Detail",
                                  new XElement("InsureeNumber", d.InsureeNumber),
                                  new XElement("ProductCode", d.ProductCode),
                                  new XElement("EnrollmentDate", DateTime.UtcNow),
                                  new XElement("IsRenewal", d.Renewal)
                                  )
                              ));

            SqlParameter[] sqlParameters = {
                new SqlParameter("@Xml", PaymentIntent.ToString()),
            };
            var data = Procedure("uspInsertPaymentIntent", sqlParameters);
        }

        public bool Valid(string InsureeNumber,string ProductCode)
        {
            return false;
        }
    }
}
