using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.DB.SqlServer.DataHelper;
using OpenImis.ePayment.Logic;
using OpenImis.ePayment.Models;
using OpenImis.ePayment.Responses;
using OpenImis.ePayment.Responses.Messages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace OpenImis.ePayment.Escape.Payment.Data
{
    public class SingleControlNumberHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private DataMessage dataMessage;
        public ePayment.Models.Language UserLanguage { get; set; }

        public SingleControlNumberHandler(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            dataMessage = new DataMessage();
        }


        public DataMessage GetControlNumber(IntentOfPay intent)
        {
            SetLanguage(intent);

            var validation = Validate(intent);

            if (validation.Code == 0)
            {
                FetchControlNumber(intent);
            }

            return dataMessage;
        }

        private void SetLanguage(IntentOfPay intent)
        {
            var languages = LocalDefault.PrimaryLanguageRepresentations(_configuration);

            if (intent.language == null || languages.Contains(intent.language.ToLower()))
            {
                UserLanguage = ePayment.Models.Language.Primary;
            }
            else
            {
                UserLanguage = ePayment.Models.Language.Secondary;
            }
        }

        public DataMessage Validate(IntentOfPay intent)
        {
            var context = new ImisDB();



            foreach (var policy in intent.policies)
            {
                // Check if the Insurance number is not blank or null
                if (String.IsNullOrEmpty(policy.insurance_number))
                {
                    dataMessage.Code = 1;
                    dataMessage.MessageValue = new Responses.Messages.Language().GetMessage((int)UserLanguage, "WrongFormatInsureeNo");
                }
                // Check if the product is valid
                else if (context.TblProduct.Where(p => p.ProductCode == policy.insurance_product_code && p.ValidityTo == null).FirstOrDefault() == null)
                {
                    dataMessage.Code = 2;
                    dataMessage.MessageValue = new Responses.Messages.Language().GetMessage((int)UserLanguage, "InValidINmissingPC");
                }
                // Check if the officer code is valid
                else if (context.TblOfficer.Where(o => o.Code == intent.enrolment_officer_code && o.ValidityTo == null).FirstOrDefault() == null)
                {
                    dataMessage.Code = 3;
                    dataMessage.MessageValue = new Responses.Messages.Language().GetMessage((int)UserLanguage, "InValidEOC");
                }
                // Check if the officer can sell this product (based on the location)
                else if (!CanOfficerSellProduct(intent.enrolment_officer_code, policy.insurance_product_code))
                {

                    dataMessage.Code = 4;
                    dataMessage.MessageValue = new Responses.Messages.Language().GetMessage((int)UserLanguage, "IncompatibleEO_PC");
                }
                // Check if the family has existing product to renew (if the Enrollment Type is renewal(1))
                else if (policy.IsRenewal() == 1 & (from prods in context.TblProduct
                                                    join pol in context.TblPolicy on prods.ProdId equals pol.ProdId
                                                    join i in context.TblInsuree on pol.FamilyId equals i.FamilyId
                                                    where (prods.ProductCode == policy.insurance_product_code
                                                    && i.Chfid == policy.insurance_number
                                                    && prods.ValidityTo == null
                                                    && i.ValidityTo == null)
                                                    select pol.PolicyId).FirstOrDefault() == 0
                         )
                {
                    dataMessage.Code = 5;
                    dataMessage.MessageValue = new Responses.Messages.Language().GetMessage((int)UserLanguage, "NoRenewalProduct");
                }

            }
            return dataMessage;
        }


        public bool CanOfficerSellProduct(string officerCode, string productCode)
        {
            var sSQL = @"SELECT P.ProdId
                        FROM tblOfficer O
                        INNER JOIN tblLocations L ON O.LocationId = L.LocationId  OR O.LocationId = L.ParentLocationId
                        INNER JOIN tblProduct P ON L.LocationId = P.LocationId OR L.ParentLocationId = P.LocationId OR P.LocationId IS NULL
                        WHERE O.ValidityTo IS NULL
                        AND L.ValidityTo IS NULL
                        AND P.ValidityTo IS NULL
                        AND O.Code = @OfficerCode
                        AND P.ProductCode = @ProductCode";

            var dh = new DataHelper(_configuration);
            SqlParameter[] parameters =
            {
                new SqlParameter("@OfficerCode", officerCode),
                new SqlParameter("@ProductCode", productCode)
            };

            var dt = dh.GetDataTable(sSQL, parameters, System.Data.CommandType.Text);
            if (dt != null && dt.Rows.Count > 0)
                return true;

            return false;
        }


        private void FetchControlNumber(IntentOfPay intent)
        {
            var sSQL = @"BEGIN TRY
	                        BEGIN TRAN CN


		                        DECLARE @PaymentId INT,
				                        @ControlNumber NVARCHAR(50),
                                        @ExpectedAmount DECIMAL(18, 2)

		                        SELECT TOP 1 @PaymentId = P.PaymentID , @ControlNumber = CN.ControlNumber, @ExpectedAmount = P.ExpectedAmount
		                        FROM tblPayment P
		                        INNER JOIN tblPaymentDetails PD ON P.PaymentId = PD.PaymentId
		                        INNER JOIN tblControlNumber CN ON P.PaymentId = CN.PaymentId
		                        WHERE P.ValidityTo IS NULL
		                        AND PD.ValidityTo IS NULL
		                        AND CN.ValidityTo IS NULL
		                        AND P.OfficerCode IS NULL
		                        AND PD.ProductCode = @ProductCode
		                        AND P.PaymentStatus = 3
		                        AND CN.ControlNumber IS NOT NULL
		                        AND PD.InsuranceNumber IS NULL
		                        
		                        --Update Phone number and Officer code
		                        UPDATE tblPayment SET OfficerCode = @OfficerCode,  PhoneNumber = @Phone, LanguageName = @LanguageName, SmsRequired = @SmsRequired
		                        WHERE PaymentID = @PaymentId;

		                        --Update Insurance number 
		                        UPDATE tblPaymentDetails SET InsuranceNumber = @InsuranceNumber, PolicyStage = CASE @EnrolmentType WHEN 1 THEN N'R' ELSE N'N' END 
		                        WHERE PaymentID = @PaymentId;


		                        SELECT @PaymentId internal_identifier, @ControlNumber control_number, @ExpectedAmount ExpectedAmount;

	                        COMMIT TRAN CN;
                    END TRY
                    BEGIN CATCH
	                    IF @@TRANCOUNT > 0
		                    ROLLBACK TRAN CN;
                    END CATCH";


            SqlParameter[] parameters =
            {
                new SqlParameter("@ProductCode", intent.policies.FirstOrDefault().insurance_product_code),
                new SqlParameter("@OfficerCode", intent.enrolment_officer_code),
                new SqlParameter("@Phone", intent.phone_number),
                new SqlParameter("@InsuranceNumber", intent.policies.FirstOrDefault().insurance_number),
                new SqlParameter("@EnrolmentType", (int)intent.policies.FirstOrDefault().renewal),
                new SqlParameter("@LanguageName", intent.language),
                new SqlParameter("@SmsRequired", intent.SmsRequired)
            };

            var dh = new DataHelper(_configuration);
            var dt = dh.GetDataTable(sSQL, parameters, System.Data.CommandType.Text);

            if (dt != null && dt.Rows.Count > 0 && !String.IsNullOrEmpty(dt.Rows[0]["control_number"].ToString()))
            {

                dataMessage = new SaveIntentResponse(0, false, dt, (int)UserLanguage).Message;
            }
        }

    }

}

