using OpenImis.ePayment.Escape.Payment.Models;
using OpenImis.ePayment.Data;
using OpenImis.ePayment.Extensions;
using OpenImis.ePayment.Models;
using OpenImis.ePayment.Models.Payment;
using OpenImis.ePayment.Models.Payment.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Data.SqlClient;
using System.Data;
using OpenImis.ePayment.Responses;

namespace OpenImis.ePayment.Data
{
    public class ImisPayment : ImisBasePayment
    {
        private IHostingEnvironment env;
        private IConfiguration config;

        public ImisPayment(IConfiguration configuration, IHostingEnvironment hostingEnvironment) : base(configuration, hostingEnvironment)
        {
            env = hostingEnvironment;
            config = configuration;
        }

#if CHF
        public async Task<object> RequestReconciliationReportAsync(int daysAgo, String productSPCode)
        {
            daysAgo = -1 * daysAgo;

            GepgUtility gepg = new GepgUtility(_hostingEnvironment, config);

            ReconcRequest Reconciliation = new ReconcRequest();

            gepgSpReconcReq request = new gepgSpReconcReq();
            request.SpReconcReqId = Math.Abs(Guid.NewGuid().GetHashCode());//Convert.ToInt32(DateTime.UtcNow.Year.ToString() + DateTime.UtcNow.Month.ToString() + DateTime.UtcNow.Day.ToString());
            request.SpCode = productSPCode;
            request.SpSysId = Configuration["PaymentGateWay:GePG:SystemId"];
            request.TnxDt = DateTime.Now.AddDays(daysAgo).ToString("yyyy-MM-dd");
            request.ReconcOpt = 1;

            var requestString = gepg.SerializeClean(request, typeof(gepgSpReconcReq));
            string signature = gepg.GenerateSignature(requestString);
            var signedRequest = gepg.FinaliseSignedMsg(new ReconcRequest() { gepgSpReconcReq = request, gepgSignature = signature }, typeof(ReconcRequest));

            var result = await gepg.SendHttpRequest("/api/reconciliations/sig_sp_qrequest", signedRequest, productSPCode, "default.sp.in");

            var content = signedRequest + "********************" + result;
            GepgFileLogger.Log(productSPCode + "_GepGReconRequest", content, env);

            return new { reconcId = request.SpReconcReqId, resp = result };

        }

        public override decimal determineTransferFee(decimal expectedAmount, TypeOfPayment typeOfPayment)
        {
            if (typeOfPayment == TypeOfPayment.BankTransfer || typeOfPayment == TypeOfPayment.Cash)
            {
                return 0;
            }
            else
            {
                var fee = expectedAmount - (expectedAmount / Convert.ToDecimal(1.011));

                return Math.Round(fee, 0);
            }
        }

        public override decimal determineTransferFeeReverse(decimal expectedAmount, TypeOfPayment typeOfPayment)
        {
            if (typeOfPayment == TypeOfPayment.BankTransfer || typeOfPayment == TypeOfPayment.Cash)
            {
                return 0;
            }
            else
            {
                var fee = expectedAmount * Convert.ToDecimal(0.011);
                return Math.Round(fee, 0);
            }
        }

        public override decimal GetToBePaidAmount(decimal ExpectedAmount, decimal TransferFee)
        {
            decimal amount = ExpectedAmount - TransferFee;
            return Math.Round(amount, 0);
        }

        public override async Task<PostReqCNResponse> PostReqControlNumberAsync(string OfficerCode, int PaymentId, string PhoneNumber, decimal ExpectedAmount, List<PaymentDetail> products, string controlNumber = null, bool acknowledge = false, bool error = false, string rejectedReason = "")
        {
            GepgUtility gepg = new GepgUtility(_hostingEnvironment, config);

            ExpectedAmount = Math.Round(ExpectedAmount, 2);
            //send request only when we have amount > 0
            if (ExpectedAmount > 0)
            {
                var bill = gepg.CreateBill(Configuration, OfficerCode, PhoneNumber, PaymentId, ExpectedAmount, GetPaymentDetails(PaymentId));

                if (bill != "-2: error - no policy")
                {
                    var signature = gepg.GenerateSignature(bill);

                    var signedMesg = gepg.FinaliseSignedMsg(signature);
                    var billAck = await gepg.SendHttpRequest("/api/bill/sigqrequest", signedMesg, gepg.GetAccountCodeByProductCode(InsureeProducts.FirstOrDefault().ProductCode), "default.sp.in");

                    string billAckRequest = JsonConvert.SerializeObject(billAck);
                    string sentbill = JsonConvert.SerializeObject(bill);

                    GepgFileLogger.Log(PaymentId, "CN_Request", sentbill + "********************" + billAckRequest, env);

                    //check if timeout in GePG server
                    if (billAck == "The operation has timed out.")
                    {
                        var rejectedReasonText = "Timeout when requesting control number";
                        return await base.PostReqControlNumberAsync(OfficerCode, PaymentId, PhoneNumber, ExpectedAmount, products, null, true, true, rejectedReasonText);
                    }

                    //get the error code from ackn GePG request
                    var errorCodes = LoadResponseCodeFromXmlAkn(billAck);
                    if (errorCodes == "7101")
                    {
                        return await base.PostReqControlNumberAsync(OfficerCode, PaymentId, PhoneNumber, ExpectedAmount, products, null, true, false);
                    }
                    else
                    {
                        //we have an error from GePG ackn - then save rejected reason
                        var rejectedReasonText = PrepareRejectedReason(PaymentId, errorCodes);
                        return await base.PostReqControlNumberAsync(OfficerCode, PaymentId, PhoneNumber, ExpectedAmount, products, null, true, true, rejectedReasonText);
                    }
                }
                else
                {
                    return await base.PostReqControlNumberAsync(OfficerCode, PaymentId, PhoneNumber, ExpectedAmount, products, null, true, true);
                }
            }
            else
            {
                //do not send any request to GePG when we have 0 or negative amount
                return await base.PostReqControlNumberAsync(OfficerCode, PaymentId, PhoneNumber, ExpectedAmount, products, null, true, true);
            }
        }

        public string ControlNumberResp(int code)
        {
            GepgUtility gepg = new GepgUtility(_hostingEnvironment, config);

            gepgBillSubRespAck CnAck = new gepgBillSubRespAck();
            CnAck.TrxStsCode = code;

            var CnAckString = gepg.SerializeClean(CnAck, typeof(gepgBillSubRespAck));
            string signature = gepg.GenerateSignature(CnAckString);
            var signedCnAck = gepg.FinaliseSignedAcks(new GepgBillResponseAck() { gepgBillSubRespAck = CnAck, gepgSignature = signature }, typeof(GepgBillResponseAck));

            return signedCnAck;
        }

        public async Task<Object> GePGPostCancelPayment(int PaymentId)
        {
            GepgUtility gepg = new GepgUtility(_hostingEnvironment, config);

            try
            {
                var GePGCancelPaymentRequest = gepg.CreateGePGCancelPaymentRequest(Configuration, PaymentId);
                string SPCode = gepg.GetAccountCodeByPaymentId(PaymentId);


                var response = await gepg.SendHttpRequest("/api/bill/sigcancel_request", GePGCancelPaymentRequest, SPCode, "changebill.sp.in");

                var content = JsonConvert.SerializeObject(GePGCancelPaymentRequest) + "\n********************\n" + JsonConvert.SerializeObject(response);
                GepgFileLogger.Log(PaymentId, "CancelPayment", content, env);

                //check if timeout in GePG server
                if (response == "The operation has timed out.")
                {
                    var rejectedReasonText = "Timeout when cancelling payment";
                    setRejectedReason(PaymentId, rejectedReasonText);
                    return new DataMessage
                    {
                        Code = -1,
                        ErrorOccured = true,
                        MessageValue = rejectedReasonText,
                    };
                }

                var errorCodes = LoadResponseCodeFromXmlAkn(response);
                if (errorCodes != "7101")
                {
                    var rejectedReasonText = PrepareRejectedReason(PaymentId, errorCodes);
                    setRejectedReason(PaymentId, rejectedReasonText);
                }

                return this.GetGePGObjectFromString(response, typeof(GePGPaymentCancelResponse));
            }
            catch (Exception ex)
            {
                return new DataMessage
                {
                    Code = -1,
                    ErrorOccured = true,
                    MessageValue = ex.ToString(),
                };
            }
        }

        public Object GetGePGObjectFromString(string input, Type type)
        {
            try
            {
                TextReader reader = new StringReader(input);
                var serializer = new XmlSerializer(type);
                return Convert.ChangeType(serializer.Deserialize(reader), type);
            }
            catch
            {
                return null;
            }

        }

        public string PaymentResp(int code)
        {
            GepgUtility gepg = new GepgUtility(_hostingEnvironment, config);

            gepgPmtSpInfoAck PayAck = new gepgPmtSpInfoAck();
            PayAck.TrxStsCode = code;

            var PayAckString = gepg.SerializeClean(PayAck, typeof(gepgPmtSpInfoAck));
            string signature = gepg.GenerateSignature(PayAckString);
            var signedPayAck = gepg.FinaliseSignedAcks(new GepgPaymentAck() { gepgPmtSpInfoAck = PayAck, gepgSignature = signature }, typeof(GepgPaymentAck));

            return signedPayAck;
        }

        public string ReconciliationResp(int code)
        {
            GepgUtility gepg = new GepgUtility(_hostingEnvironment, config);

            gepgSpReconcRespAck ReconcAck = new gepgSpReconcRespAck();
            ReconcAck.ReconcStsCode = code;

            var ReconcAckString = gepg.SerializeClean(ReconcAck, typeof(gepgSpReconcRespAck));
            string signature = gepg.GenerateSignature(ReconcAckString);
            var signedReconcAck = gepg.FinaliseSignedAcks(new GepgReconcRespAck() { gepgSpReconcRespAck = ReconcAck, gepgSignature = signature }, typeof(GepgReconcRespAck));

            return signedReconcAck;
        }

        public List<String> GetProductsSPCode()
        {

            var getProductsSPCodes = @"SELECT DISTINCT tblProduct.AccCodePremiums FROM tblProduct WHERE tblProduct.AccCodePremiums LIKE 'SP[0-9][0-9][0-9]' AND tblProduct.ValidityTo is NULL";

            SqlParameter[] parameters = { };

            try
            {
                DataTable results = dh.GetDataTable(getProductsSPCodes, parameters, CommandType.Text);
                List<String> productsCodes = new List<String>();
                if (results.Rows.Count > 0)
                {
                    foreach (DataRow result in results.Rows)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(result["AccCodePremiums"])))
                        {
                            productsCodes.Add(Convert.ToString(result["AccCodePremiums"]));
                        }
                    }
                }

                return productsCodes;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public object GetPaymentToReconciliate(ReconcTrxInf payment)
        {
            object result = null;
            double paidAmount = Convert.ToDouble(payment.PaidAmt);
            SqlParameter[] parameters = {
                new SqlParameter("@Id", payment.SpBillId),
                new SqlParameter("@paidAmount", payment.PaidAmt),
                new SqlParameter("@PaymentStatus", PaymentStatus.Matched),
            };

            var sSQL = @"SELECT PaymentId, ExpectedAmount, ReceivedAmount, PaymentStatus FROM tblPayment WHERE PaymentId=@Id And PaymentStatus<=@PaymentStatus And ExpectedAmount=@paidAmount And ValidityTo is Null";

            try
            {
                var data = dh.GetDataTable(sSQL, parameters, CommandType.Text);

                if (data.Rows.Count > 0)
                {
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        var rw = data.Rows[i];
                        var expectedAmount = rw["ExpectedAmount"] != System.DBNull.Value ? Convert.ToDouble(rw["ExpectedAmount"]) : 0;
                        var receivedAmount = rw["ReceivedAmount"] != System.DBNull.Value ? Convert.ToDouble(rw["ReceivedAmount"]) : 0;
                        if (paidAmount == expectedAmount && receivedAmount == paidAmount)
                        {
                            result = new
                            {
                                paymentId = rw["PaymentID"].ToString(),
                                expectedAmount = expectedAmount,
                                receivedAmount = receivedAmount,
                                paymentStatus = rw["PaymentStatus"],
                            };
                        }
                    }
                }
            }
            catch (Exception e)
            { }

            return result;
        }

        private string LoadResponseCodeFromXmlAkn(string xmlContent)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);

            XmlNodeList errorCodeTag = doc.GetElementsByTagName("TrxStsCode");
            if (errorCodeTag.Count < 1)
            {
                return "";
            }
            else
            {
                string errorCode = errorCodeTag[0].InnerText;
                return errorCode;
            }
        }

        public string PrepareRejectedReason(int billId, string errorCodes = "7101")
        {
            //prepare to save RejectedReason column the error codes and short description of error from GePG
            var rejectedReason = "";
            if (errorCodes != "7101")
            {
                //split error codes
                var listOfErrors = errorCodes.Split(';');
                for (var i = 0; i < listOfErrors.Length; i++)
                {
                    if (i != listOfErrors.Length - 1)
                    {
                        rejectedReason += listOfErrors[i] + ":" + GepgCodeResponses.GepgResponseCodes.FirstOrDefault(x => x.Value == int.Parse(listOfErrors[i])).Key + ";";
                    }
                    else
                    {
                        rejectedReason += listOfErrors[i] + ":" + GepgCodeResponses.GepgResponseCodes.FirstOrDefault(x => x.Value == int.Parse(listOfErrors[i])).Key;
                    }
                }
            }
            return rejectedReason;
        }

        public async Task<string> RequestBulkControlNumbers(RequestBulkControlNumbersModel model)
        {
            var gepg = new GepgUtility(_hostingEnvironment, Configuration);
            var bills = await gepg.CreateBulkBills(Configuration, model);

            var signature = gepg.GenerateSignature(bills);
            var signedMesg = gepg.FinaliseSignedMsg(signature);

            string accountCode = gepg.GetAccountCodeByProductCode(model.ProductCode);

            var billAck = await gepg.SendHttpRequest("/api/bill/sigqrequest", signedMesg, accountCode, "default.sp.in");
            return billAck;
        }

        public List<BulkControlNumbersForEO> GetControlNumbersForEO(string officerCode, string productCode)
        {
            var bulkControlNumbers = new List<BulkControlNumbersForEO>();

            var sSQL = @"SET TRANSACTION ISOLATION LEVEL READ COMMITTED;

                        BEGIN TRANSACTION;
	                        DECLARE @dt TABLE
	                        (
		                        ControlNumberId INT,
		                        BillId INT,
		                        ProductCode NVARCHAR(50),
		                        OfficerCode NVARCHAR(50),
		                        ControlNumber NVARCHAR(15),
		                        Amount DECIMAL(18, 2)
	                        )
	                        INSERT INTO @dt(ControlNumberId, BillId, ProductCode, OfficerCode, ControlNumber, Amount)

	                        SELECT TOP 100 CN.ControlNumberID, CN.[PaymentID] BillId, PD.ProductCode, @OfficerCode OfficerCode, CN.[ControlNumber], PD.ExpectedAmount Amount
	                        FROM tblControlNumber CN
	                        INNER JOIN tblPaymentDetails PD ON CN.PaymentID = PD.PaymentID
	                        INNER JOIN tblPayment P ON PD.PaymentID = P.PaymentID
	                        WHERE CN.ControlNumber IS NOT NULL
	                        AND PD.ProductCode = @ProductCode
	                        AND P.OfficerCode IS NULL
	                        AND CN.ValidityTo IS NULL
	                        AND PD.ValidityTo IS NULL
	                        AND P.ValidityTo IS NULL;

	                        UPDATE P SET OfficerCode = @OfficerCode
	                        FROM @dt dt
	                        INNER JOIN tblControlNumber CN ON dt.ControlNumberId = CN.ControlNumberID
	                        INNER JOIN tblPaymentDetails PD ON CN.PaymentID = PD.PaymentID
	                        INNER JOIN tblPayment P ON PD.PaymentID = P.PaymentID
	                        WHERE CN.ControlNumber IS NOT NULL
	                        AND PD.ProductCode = @ProductCode
	                        AND P.OfficerCode IS NULL
	                        AND CN.ValidityTo IS NULL
	                        AND PD.ValidityTo IS NULL
	                        AND P.ValidityTo IS NULL;

	                        SELECT ControlNumberId, BillId, ProductCode, OfficerCode, ControlNumber, Amount FROM @dt;

                        COMMIT TRANSACTION;";

            var dh = new DataHelper(config);
            SqlParameter[] parameters = {
                new SqlParameter("@OfficerCode", officerCode),
                new SqlParameter("@ProductCode", productCode)
            };

            var dt = dh.GetDataTable(sSQL, parameters, CommandType.Text);

            foreach (DataRow dr in dt.Rows)
            {
                var controlNumber = new BulkControlNumbersForEO
                {
                    ControlNumberId = Convert.ToInt32(dr["ControlNumberID"]),
                    BillId = Convert.ToInt32(dr["BillId"]),
                    ProductCode = dr["ProductCode"].ToString(),
                    OfficerCode = dr["OfficerCode"].ToString(),
                    ControlNumber = dr["ControlNumber"].ToString(),
                    Amount = (decimal)dr["Amount"],
                };

                bulkControlNumbers.Add(controlNumber);
            }

            return bulkControlNumbers;

        }

        public int ControlNumbersToBeRequested(string productCode)
        {
            var sSQL = @";WITH TotalProductUsage
                        AS
                        (
	                        SELECT CASE WHEN COUNT(1)/3 < 200 THEN 200 ELSE COUNT(1)/3 END  Last3MonthsEnrollment 
	                        FROM tblPolicy PL
	                        INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID
	                        WHERE PL.ValidityTo IS NULL
	                        AND Prod.ValidityTo IS NULL
	                        AND Prod.ProductCode = @ProductCode
	                        AND PL.EnrollDate BETWEEN DATEADD(MONTH, -3, GETDATE()) AND GETDATE()
                        ), RemainingCNs AS
                        (
	                        SELECT COUNT(1) ControlNumbersLeft
	                        FROM tblControlNumber CN
	                        INNER JOIN tblPayment P ON CN.PaymentID = P.PaymentID
	                        INNER JOIN tblPaymentDetails PD ON P.PaymentID = PD.PaymentID
	                        WHERE CN.ValidityTo IS NULL
	                        AND P.ValidityTo IS NULL
	                        AND PD.ProductCode = @ProductCode
	                        AND P.OfficerCode IS NULL
	                        AND CN.ControlNumber IS NOT NULL
                        )
                        SELECT CASE WHEN Last3MonthsEnrollment > ControlNumbersLeft THEN Last3MonthsEnrollment - ControlNumbersLeft  ELSE 0 END NeedToRequest
                        FROM RemainingCNs, TotalProductUsage";

            SqlParameter[] parameters = {
                new SqlParameter("@ProductCode", productCode)
                };

            var dh = new DataHelper(config);
            var dt = dh.GetDataTable(sSQL, parameters, CommandType.Text);

            int needToRequest = 0;

            if (dt.Rows.Count > 0)
                needToRequest = (int)dt.Rows[0]["NeedToRequest"];

            return needToRequest;
        }

#endif
    }
}
