using OpenImis.ePayment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenImis.ePayment.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using OpenImis.ePayment.Models.Payment;
using OpenImis.ePayment.Responses;
using OpenImis.ePayment.Models.Sms;
using OpenImis.ePayment.Escape.Sms;
using Newtonsoft.Json;
using OpenImis.ePayment.Models.Payment.Response;
using System.IO;
using System.Xml.Serialization;
using OpenImis.ePayment.Escape.Payment.Models;
using OpenImis.DB.SqlServer;
using OpenImis.ePayment.Escape.Payment.Data;
using Microsoft.Extensions.Logging;

namespace OpenImis.ePayment.Logic
{
    public class PaymentLogic
    {

        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILoggerFactory _loggerFactory;

        public PaymentLogic(IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _loggerFactory = loggerFactory;

        }
        public async Task<DataMessage> SaveIntent(IntentOfPay intent, int? errorNumber = 0, string errorMessage = null)
        {
            var payment = new ImisPayment(_configuration, _hostingEnvironment, _loggerFactory);
            var cnHandler = new SingleControlNumberHandler(_configuration, _hostingEnvironment);
            var return_message = cnHandler.GetControlNumber(intent);
            var data = new AssignedControlNumber();
            if (return_message.ErrorOccured == true)
                data = (AssignedControlNumber)return_message.Data;

            if (return_message.ErrorOccured == false)
            {
                var objstring = return_message.Data.ToString();
                data = (JsonConvert.DeserializeObject<List<AssignedControlNumber>>(objstring)).FirstOrDefault();
                return_message.Data = data;
            }
            

            if (data.control_number != null & data.internal_identifier != null && intent.SmsRequired & !return_message.ErrorOccured)
            {
                payment.GetPaymentInfo(Convert.ToInt32(data.internal_identifier));
                ControlNumberAssignedSms(payment);
            }
            else
            {
                payment.ControlNum = null;
                payment.InsureeProducts = new List<InsureeProduct>();
                payment.InsureeProducts.Add(
                    new InsureeProduct { 
                        InsureeNumber = intent.policies.FirstOrDefault().insurance_number,
                        ProductCode = intent.policies.FirstOrDefault().insurance_product_code
                    }
                );
                payment.PhoneNumber = intent.phone_number;
                ControlNumberNotassignedSms(payment, return_message.MessageValue);
            }

            return return_message;
        }

        public async Task<DataMessage> MatchPayment(MatchModel model)
        {
            ImisPayment payment = new ImisPayment(_configuration, _hostingEnvironment, _loggerFactory);
            var response = payment.MatchPayment(model);

            if (model.internal_identifier == 0)
            {
                List<MatchSms> PaymentIds = new List<MatchSms>();

                PaymentIds = payment.GetPaymentIdsForSms();

                if (PaymentIds != null)
                {
                    // SendMatchSms(PaymentIds);
                }
            }
            else
            {
                // XML TO JSON
                var matchdata = JsonConvert.SerializeObject(response.Data);
                //
                var matchedPayments = JsonConvert.DeserializeObject<List<MatchedPayment>>(matchdata);

                if (matchedPayments.FirstOrDefault().PaymentMatched > 0)
                {
                    //SendPaymentSms(payment);
                }

            }

            return await Task.FromResult(response);
        }

        public async Task<DataMessage> SaveAcknowledgementAsync(Acknowledgement model)
        {
            ImisPayment payment = new ImisPayment(_configuration, _hostingEnvironment, _loggerFactory) { PaymentId = model.internal_identifier };
            var response = await payment.SaveControlNumberAknAsync(model.error_occured, model.error_message);

            return response;
        }

        public async Task<DataMessage> SavePayment(PaymentData model)
        {

            ImisPayment payment = new ImisPayment(_configuration, _hostingEnvironment, _loggerFactory);
            //var controlNumberExists = payment.CheckControlNumber(model.internal_identifier, model.control_number);

            if (model.control_number != null)
            {
                model.type_of_payment = null;

                int paymentId = payment.GetPaymentId(model.control_number);

                if (paymentId != 0)
                {
                    payment.GetPaymentInfo(paymentId);
                }
                else
                {
                    DataMessage dm = new DataMessage
                    {
                        Code = 3,
                        ErrorOccured = true,
                        MessageValue = "3-Wrong control_number",
                    };

                    return dm;
                }
            }

            if (model.type_of_payment == null && payment.typeOfPayment != null)
            {
                var transferFee = payment.determineTransferFeeReverse(Convert.ToDecimal(model.received_amount), (TypeOfPayment)payment.typeOfPayment);
                var success = await payment.UpdatePaymentTransferFeeAsync(payment.PaymentId, transferFee, (TypeOfPayment)payment.typeOfPayment);
                model.received_amount = model.received_amount + Convert.ToDouble(transferFee);
            }
            else if (model.type_of_payment != null && payment.typeOfPayment == null)
            {
                var transferFee = payment.determineTransferFeeReverse(Convert.ToDecimal(model.received_amount), (TypeOfPayment)model.type_of_payment);
                var success = await payment.UpdatePaymentTransferFeeAsync(payment.PaymentId, transferFee, (TypeOfPayment)model.type_of_payment);
                model.received_amount = model.received_amount + Convert.ToDouble(transferFee);
            }

            var response = await payment.SavePaymentAsync(model);

            if (payment.PaymentId != 0)
            {

                if (_configuration.GetValue<bool>("PaymentGateWay:CreatePremiumOnPaymentReceived") & response.Code == 0)
                    CreatePremium(payment.PaymentId);

                SendPaymentConfirmationSms(model, payment);
            }

            if (payment.PaymentId != 0 && !response.ErrorOccured)
            {

                var ackResponse = payment.GetPaymentDataAck(payment.PaymentId, payment.ControlNum);

                MatchModel matchModel = new MatchModel() { internal_identifier = payment.PaymentId, audit_user_id = -3 };

                var matchresponse = await MatchPayment(matchModel);
                /*
#if DEBUG
                var matchdata = JsonConvert.SerializeObject(matchresponse.Data);
                var matchedPayments = JsonConvert.DeserializeObject<List<MatchedPayment>>(matchdata);

                if (matchedPayments.Select(x => x.PaymentId).Contains(payment.PaymentId))
                {
                    SendPaymentSms(payment);
                }
#endif*/

            }

            return response;
        }

        public async Task<DataMessage> SaveControlNumberAsync(ControlNumberResp model)
        {

            ImisPayment payment = new ImisPayment(_configuration, _hostingEnvironment, _loggerFactory);
            var controlNumberExists = payment.CheckControlNumber(model.internal_identifier, model.control_number);
            var response = await payment.SaveControlNumberAsync(model, controlNumberExists);

            if (model.error_occured)
            {
                response.ErrorOccured = true;
                response.MessageValue = model.error_message;
            }

            if (payment.PaymentId != 0 && payment.SmsRequired)
            {
                var ackResponse = payment.GetReqControlNumberAck(payment.PaymentId);

                if (!response.ErrorOccured && !controlNumberExists)
                {
                    ControlNumberAssignedSms(payment);
                }
                else
                {
                    ControlNumberNotassignedSms(payment, response.MessageValue);
                }
            }

            return response;
        }

        public async void ControlNumberAssignedSms(ImisPayment payment)
        {
            //Language lang = payment.Language.ToLower() == "en" || payment.Language.ToLower() == "english" || payment.Language.ToLower() == "primary" ? Language.Primary : Language.Secondary;
            ImisSms sms = new ImisSms(_configuration, _hostingEnvironment, payment.Language);
            var txtmsgTemplate = string.Empty;
            string othersCount = string.Empty;

            if (payment.InsureeProducts.Count > 1)
            {
                txtmsgTemplate = sms.GetMessage("ControlNumberAssignedV2");
                othersCount = Convert.ToString(payment.InsureeProducts.Count - 1);
            }
            else
            {
                txtmsgTemplate = sms.GetMessage("ControlNumberAssigned");
            }

            decimal transferFee = 0;

            if (payment.typeOfPayment != null)
            {
                transferFee = payment.determineTransferFee(payment.ExpectedAmount, (TypeOfPayment)payment.typeOfPayment);

            }

            var txtmsg = string.Format(txtmsgTemplate,
                payment.ControlNum,
                DateTime.UtcNow.ToString("dd-MM-yyyy"),
                DateTime.UtcNow.ToString("dd-MM-yyyy"),
                payment.InsureeProducts.FirstOrDefault().InsureeNumber,
                payment.InsureeProducts.FirstOrDefault().InsureeName,
                payment.InsureeProducts.FirstOrDefault().ProductCode,
                payment.InsureeProducts.FirstOrDefault().ProductName,
                payment.GetToBePaidAmount(payment.ExpectedAmount, transferFee),
                othersCount);

            List<SmsContainer> message = new List<SmsContainer>();
            message.Add(new SmsContainer() { Message = txtmsg, Recipient = payment.PhoneNumber });

            var fileName = "CnAssigned_" + payment.PhoneNumber;

            string test = await sms.SendSMS(message, fileName);
        }

        public async Task<DataMessage> GetControlNumbers(PaymentRequest requests)
        {
            ImisPayment payment = new ImisPayment(_configuration, _hostingEnvironment, _loggerFactory);
            var PaymentIds = requests.requests.Select(x => x.internal_identifier).ToArray();

            var PaymentIds_string = string.Join(",", PaymentIds);

            var response = await payment.GetControlNumbers(PaymentIds_string);

            return response;
        }

        public async void ControlNumberNotassignedSms(ImisPayment payment, string error)
        {
            //Language lang = payment.Language.ToLower() == "en" || payment.Language.ToLower() == "english" || payment.Language.ToLower() == "primary" ? Language.Primary : Language.Secondary;
            ImisSms sms = new ImisSms(_configuration, _hostingEnvironment, payment.Language);
            var txtmsgTemplate = string.Empty;
            string othersCount = string.Empty;

            if (payment.InsureeProducts.Count > 1)
            {
                txtmsgTemplate = sms.GetMessage("ControlNumberErrorV2");
                othersCount = Convert.ToString(payment.InsureeProducts.Count - 1);
            }
            else
            {
                txtmsgTemplate = sms.GetMessage("ControlNumberError");
            }

            var txtmsg = string.Format(txtmsgTemplate,
                payment.ControlNum,
                DateTime.UtcNow.ToString("dd-MM-yyyy"),
                DateTime.UtcNow.ToString("dd-MM-yyyy"),
                payment.InsureeProducts.FirstOrDefault().InsureeNumber,
                payment.InsureeProducts.FirstOrDefault().ProductCode,
                error,
                othersCount);

            List<SmsContainer> message = new List<SmsContainer>();
            message.Add(new SmsContainer() { Message = txtmsg, Recipient = payment.PhoneNumber });

            var fileName = "CnError_" + payment.PhoneNumber;

            string test = await sms.SendSMS(message, fileName);
        }

        public async void SendPaymentConfirmationSms(PaymentData pd, ImisPayment payment)
        {
            // Language lang = payment.Language.ToLower() == "en" || payment.Language.ToLower() == "english" || payment.Language.ToLower() == "primary" ? Language.Primary : Language.Secondary;
            ImisSms sms = new ImisSms(_configuration, _hostingEnvironment, payment.Language);
            List<SmsContainer> message = new List<SmsContainer>();

            var txtmsg = string.Format(
                sms.GetMessage("PaymentConfirmationSMS"), // template
                payment.Location, // Product location 
                pd.control_number, // invoice number
                pd.received_amount, // amount paid
                pd.receipt_identification, // receipt number
                DateTime.Parse(pd.payment_date).ToString(), // payment date
                pd.transaction_identification // transaction number 
                );


            message.Add(new SmsContainer() { Message = txtmsg, Recipient = pd.payer_phone_number });


            var fileName = "PaymentConfirmationSms_" + pd.payer_phone_number;

            string test = await sms.SendSMS(message, fileName);
            payment.UpdateLastSMSSentDateAsync();
        }

        public async void SendPaymentSms(ImisPayment payment)
        {
            // Language lang = payment.Language.ToLower() == "en" || payment.Language.ToLower() == "english" || payment.Language.ToLower() == "primary" ? Language.Primary : Language.Secondary;
            ImisSms sms = new ImisSms(_configuration, _hostingEnvironment, payment.Language);
            List<SmsContainer> message = new List<SmsContainer>();
            var familyproduct = payment.InsureeProducts.FirstOrDefault();

            if (familyproduct.PolicyActivated)
            {

                var txtmsg = string.Format(sms.GetMessage("PaidAndActivated"),
                    payment.PaidAmount,
                    DateTime.UtcNow.ToString("dd-MM-yyyy"),
                    payment.ControlNum,
                    familyproduct.InsureeNumber,
                    familyproduct.InsureeName,
                    familyproduct.ProductCode,
                    familyproduct.ProductName,
                    familyproduct.EffectiveDate.Value.ToShortDateString(),
                    familyproduct.ExpiryDate.Value.ToShortDateString(),
                    payment.PaidAmount);


                message.Add(new SmsContainer() { Message = txtmsg, Recipient = payment.PhoneNumber });

            }
            else
            {

                decimal transferFee = 0;

                if (payment.typeOfPayment != null)
                {
                    transferFee = payment.determineTransferFee(payment.ExpectedAmount, (TypeOfPayment)payment.typeOfPayment);
                }

                var txtmsg = string.Format(sms.GetMessage("PaidAndNotActivated"),
                    payment.PaidAmount,
                    DateTime.UtcNow.ToString("dd-MM-yyyy"),
                    payment.ControlNum,
                    familyproduct.InsureeNumber,
                    familyproduct.InsureeName,
                    familyproduct.ProductCode,
                    familyproduct.ProductName,
                    payment.GetToBePaidAmount(payment.ExpectedAmount, transferFee),
                    payment.OutStAmount);

                message.Add(new SmsContainer() { Message = txtmsg, Recipient = payment.PhoneNumber });

            }
            var fileName = "PayStatSms_" + payment.PhoneNumber;

            string test = await sms.SendSMS(message, fileName);
            payment.UpdateLastSMSSentDateAsync();
        }

        public async void SendMatchSms(ImisPayment payment)
        {
            // Language lang = payment.Language.ToLower() == "en" || payment.Language.ToLower() == "english" || payment.Language.ToLower() == "primary" ? Language.Primary : Language.Secondary;
            ImisSms sms = new ImisSms(_configuration, _hostingEnvironment, payment.Language);
            List<SmsContainer> message = new List<SmsContainer>();

            var txtmsgTemplate = string.Empty;
            string othersCount = string.Empty;

            if (payment.InsureeProducts.Count > 1)
            {
                txtmsgTemplate = sms.GetMessage("PaidAndNotMatchedV2");
                othersCount = Convert.ToString(payment.InsureeProducts.Count - 1);
            }
            else
            {
                txtmsgTemplate = sms.GetMessage("PaidAndNotMatched");
            }
            var familyproduct = payment.InsureeProducts.FirstOrDefault();
            var txtmsg = string.Format(txtmsgTemplate,
                     payment.PaidAmount,
                     DateTime.UtcNow.ToString("dd-MM-yyyy"),
                     payment.ControlNum,
                     familyproduct.InsureeNumber,
                     familyproduct.InsureeName,
                     familyproduct.ProductCode,
                     familyproduct.ProductName,
                     othersCount);


            message.Add(new SmsContainer() { Message = txtmsg, Recipient = payment.PhoneNumber });

            var fileName = "PayNotMatched_" + payment.PhoneNumber;

            string test = await sms.SendSMS(message, fileName);
        }

        public async void SendMatchSms(List<MatchSms> Ids)
        {

            List<SmsContainer> message = new List<SmsContainer>();

            foreach (var m in Ids)
            {
                bool shoulSendSms = LocalDefault.ShouldSendSms(_configuration, m.DateLastSms, m.MatchedDate);

                if (shoulSendSms)
                {
                    var txtmsgTemplate = string.Empty;
                    string othersCount = string.Empty;

                    ImisPayment _pay = new ImisPayment(_configuration, _hostingEnvironment, _loggerFactory);
                    _pay.GetPaymentInfo(m.PaymentId);

                    //Language lang = _pay.Language.ToLower() == "en" || _pay.Language.ToLower() == "english" || _pay.Language.ToLower() == "primary" ? Language.Primary : Language.Secondary;
                    ImisSms sms = new ImisSms(_configuration, _hostingEnvironment, _pay.Language);

                    if (_pay.PaymentId != 0)
                    {
                        if (_pay.InsureeProducts.Count > 1)
                        {
                            txtmsgTemplate = sms.GetMessage("PaidAndNotMatchedV2");
                            othersCount = Convert.ToString(_pay.InsureeProducts.Count - 1);
                        }
                        else
                        {
                            txtmsgTemplate = sms.GetMessage("PaidAndNotMatched");
                        }
                        var familyproduct = _pay.InsureeProducts.FirstOrDefault();
                        var txtmsg = string.Format(txtmsgTemplate,
                                 _pay.PaidAmount,
                                 DateTime.UtcNow.ToString("dd-MM-yyyy"),
                                 _pay.ControlNum,
                                 familyproduct.InsureeNumber,
                                 familyproduct.InsureeName,
                                 familyproduct.ProductCode,
                                 familyproduct.ProductName,
                                 othersCount);


                        message.Add(new SmsContainer() { Message = txtmsg, Recipient = _pay.PhoneNumber });
                        _pay.UpdateLastSMSSentDateAsync();
                    }
                    else
                    {
                        throw new Exception();
                    }

                    var fileName = "PayNotMatched_";
                    string test = await sms.SendSMS(message, fileName);
                }

            }

        }

        public async void SendPaymentCancellationSms(ImisPayment payment)
        {
            ImisSms sms = new ImisSms(_configuration, _hostingEnvironment, payment.Language);
            List<SmsContainer> message = new List<SmsContainer>();

            var txtmsg = string.Format(
                sms.GetMessage("CancellationSms"), // template
                payment.ControlNum, // invoice number
                DateTime.Now.ToString() // payment cancellation date  
                );

            message.Add(new SmsContainer() { Message = txtmsg, Recipient = payment.PhoneNumber });

            var fileName = "PaymentCancellationSms_" + payment.PhoneNumber;

            string test = await sms.SendSMS(message, fileName);
            payment.UpdateLastSMSSentDateAsync();
        }

        public async Task<ReconciliationMessage> ProvideReconciliationData(ReconciliationRequest model)
        {
            ImisPayment payment = new ImisPayment(_configuration, _hostingEnvironment, _loggerFactory);

            var response = payment.ProvideReconciliationData(model);

            ReconciliationMessage return_message = new ReconciliationMessage
            {
                transactions = response,
                error_occurred = false
            };

            return await Task.FromResult(return_message);
        }

        // Todo: make this method generic
        public async Task<DataMessage> CancelPayment(int payment_id)
        {

            ImisPayment payment = new ImisPayment(_configuration, _hostingEnvironment, _loggerFactory);
            DataMessage dt = new DataMessage();

            if (payment_id > 0)
            {
                payment.GetPaymentInfo(payment_id);
                await payment.CancelPayment(payment_id);
                SendPaymentCancellationSms(payment);
            }
            else
            {
                //Todo: move hardcoded message to translation file
                DataMessage dm = new DataMessage
                {
                    Code = 2,
                    ErrorOccured = true,
                    MessageValue = "CancelPayment:2:Control Number doesn't exists",
                };

                return dm;
            }

            return dt;
        }


        public TblOfficer GetOfficerInfo(int officerId)
        {
            var imisPayment = new ImisBasePayment(_configuration, _hostingEnvironment, _loggerFactory);
            return imisPayment.GetOfficerInfo(officerId);
        }

        public async Task<string> RequestBulkControlNumbers(RequestBulkControlNumbersModel model)
        {
            var imisPayment = new ImisPayment(_configuration, _hostingEnvironment, _loggerFactory);
            return await imisPayment.RequestBulkControlNumbers(model);

        }

        public BulkControlNumbersForEO GetControlNumbersForEO(string officerCode, string productCode, int available)
        {
            var imisPayment = new ImisPayment(_configuration, _hostingEnvironment, _loggerFactory);

            var  threshold = Convert.ToInt32( _configuration["PaymentGateWay:MaxControlNumberForEO"]);

            var requiredCNs =  threshold - available;

            if (requiredCNs <= 0)
            {
                var response = new BulkControlNumbersForEO();
                var header = new ControlNumbersForEOHeader
                {
                    Error = (int)Errors.ControlNumbers.ThresholdReached,
                    ErrorMessage = Errors.ControlNumbers.ThresholdReached.ToString()

                };

                var controlNumebrs = new List<ControlNumbersForEO>();

                response.Header = header;
                response.ControlNumbers = controlNumebrs;

                return response;

            }

            return imisPayment.GetControlNumbersForEO(officerCode, productCode, requiredCNs);
        }

        public async Task<int> ControlNumbersToBeRequested(string productCode)
        {
            var imisPayment = new ImisPayment(_configuration, _hostingEnvironment, _loggerFactory);
            return await imisPayment.ControlNumbersToBeRequested(productCode);
        }

        public int CreatePremium(int paymentId)
        {
            var imisPayment = new ImisPayment(_configuration, _hostingEnvironment, _loggerFactory);
            return imisPayment.CreatePremium(paymentId);
        }

        public async Task<bool> HandleControlNumbersToBeRequested(string productCode)
        {
            try
            {
                var count = await ControlNumbersToBeRequested(productCode);
                if (count > 0)
                {
                    // The following method is Async, but we do not await it since we don't want to wait for the result
                    _ = RequestBulkControlNumbers(new RequestBulkControlNumbersModel { ControlNumberCount = count, ProductCode = productCode });
                }
            }
            catch (Exception)
            {

            }

            return true;
        }

    }
}
