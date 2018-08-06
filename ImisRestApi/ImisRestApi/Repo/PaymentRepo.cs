using System;
using System.Collections.Generic;
using System.Data;
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
        public float ExpectedAmount { get; set; }

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
                    new XElement("RequestDate", _intent.Request_Date.ToShortDateString()),
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
                                  new XElement("IsRenewal", Convert.ToInt32(d.Renewal))
                                  )
                              ));


            SqlParameter[] sqlParameters = {
                new SqlParameter("@Xml", PaymentIntent.ToString()),
                new SqlParameter("@PaymentID", SqlDbType.Int){Direction = ParameterDirection.Output },
                new SqlParameter("@ExpectedAmount", SqlDbType.Int){Direction = ParameterDirection.Output }
             };
            var data = Procedure("uspInsertPaymentIntent", sqlParameters);

            PaymentId = Convert.ToInt32(data[0].Value.ToString());
            ExpectedAmount = float.Parse(data[1].Value.ToString());

        }

        public void SaveControlNumber()
        {
            SqlParameter[] sqlParameters = {
                new SqlParameter("@PaymentID", PaymentId),
                new SqlParameter("@RequestOrigin", "IMIS")
             };

            var data = Procedure("uspRequestGetControlNumber", sqlParameters);
        }

        public void SaveControlNumber(string ControlNumber)
        {
            SqlParameter[] sqlParameters = {
                new SqlParameter("@PaymentID", PaymentId),
                new SqlParameter("@RequestOrigin", "IMIS"),
                new SqlParameter("@ControlNumber", "IMIS")
             };

            var data = Procedure("uspReceiveControlNumber", sqlParameters);
        }

        public void SaveControlNumberAkn(bool Success, string Comment)
        {
            XElement CNAcknowledgement = new XElement("ControlNumberAcknowledge",
                  new XElement("PaymentID", PaymentId),
                  new XElement("Success", Success),
                  new XElement("Comment", Comment)
                  );

            SqlParameter[] sqlParameters = {
                new SqlParameter("@Xml", CNAcknowledgement)
             };

            var data = Procedure("uspAcknowledgeControlNumberRequest", sqlParameters);
        }

        public bool Valid(string InsureeNumber,string ProductCode)
        {
            return false;
        }
    }
}
