using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenImis.ModulesV1.PaymentModule.Helpers;
using OpenImis.ModulesV1.PaymentModule.Helpers.SMS;
using OpenImis.ModulesV1.PaymentModule.Models;
using OpenImis.ModulesV1.PaymentModule.Models.Response;
using OpenImis.ModulesV1.PaymentModule.Models.SMS;
using OpenImis.ModulesV1.PaymentModule.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ModulesV1.PaymentModule.Logic
{
    public class PaymentLogic : IPaymentLogic
    {
        private IConfiguration Configuration;

        protected IPaymentRepository paymentRepository;

        public string WebRootPath { get; set; }
        public string ContentRootPath { get; set; }

        public PaymentLogic(IConfiguration configuration)
        {
            Configuration = configuration;
            paymentRepository = new PaymentRepository(Configuration);
        }

        public async Task<DataMessage> SaveIntent(IntentOfPay intent, int? errorNumber = 0, string errorMessage = null)
        {
            IPaymentRepository payment = new PaymentRepository(Configuration);
            var intentResponse = payment.SaveIntent(intent, errorNumber, errorMessage);

            DataMessage return_message = new DataMessage();
            return_message.Code = intentResponse.Code;
            return_message.MessageValue = intentResponse.MessageValue;

            if (intentResponse.Code == 0)
            {
                var objstring = intentResponse.Data.ToString();
                List<AssignedControlNumber> data = JsonConvert.DeserializeObject<List<AssignedControlNumber>>(objstring);
                var ret_data = data.FirstOrDefault();

                decimal transferFee = 0;
                //Get the transfer Fee
                if (intent.type_of_payment != null)
                {
                    transferFee = payment.determineTransferFee(payment.ExpectedAmount, (TypeOfPayment)intent.type_of_payment);

                    var success = payment.UpdatePaymentTransferFee(payment.PaymentId, transferFee, (TypeOfPayment)intent.type_of_payment);

                }

                var amountToBePaid = payment.GetToBePaidAmount(payment.ExpectedAmount, transferFee);
                var response = payment.PostReqControlNumber(intent.enrolment_officer_code, payment.PaymentId, intent.phone_number, amountToBePaid, intent.policies);

                if (response.ControlNumber != null)
                {
                    var controlNumberExists = payment.CheckControlNumber(payment.PaymentId, response.ControlNumber);
                    return_message = payment.SaveControlNumber(response.ControlNumber, controlNumberExists);
                    if (payment.PaymentId != null)
                    {
                        if (!return_message.ErrorOccured && !controlNumberExists)
                        {
                            ret_data.control_number = response.ControlNumber;
                            ControlNumberAssignedSms(payment);
                        }
                        else
                        {
                            ControlNumberNotassignedSms(payment, return_message.MessageValue);
                        }
                    }
                }
                else if (response.Posted == true)
                {
                    return_message = payment.SaveControlNumberAkn(response.ErrorOccured, response.ErrorMessage);
                }
                else if (response.ErrorOccured == true)
                {
                    return_message = payment.SaveControlNumberAkn(response.ErrorOccured, response.ErrorMessage);
                    ControlNumberNotassignedSms(payment, response.ErrorMessage);

                }

                return_message.Data = ret_data;
            }
            else
            {
                return_message = intentResponse;
                return_message.Data = new AssignedControlNumber();
            }

            return return_message;
        }

        public async Task<DataMessage> MatchPayment(MatchModel model)
        {
            PaymentRepository payment = new PaymentRepository(Configuration);
            var response = payment.MatchPayment(model);

            List<MatchSms> PaymentIds = new List<MatchSms>();

            PaymentIds = payment.GetPaymentIdsForSms();

            if (PaymentIds != null)
                SendMatchSms(PaymentIds);

            return response;
        }

        public DataMessage SaveAcknowledgement(Acknowledgement model)
        {
            PaymentRepository payment = new PaymentRepository(Configuration) { PaymentId = model.internal_identifier };
            var response = payment.SaveControlNumberAkn(model.error_occured, model.error_message);

            return response;
        }

        public async Task<DataMessage> SavePayment(PaymentData model)
        {

            PaymentRepository payment = new PaymentRepository(Configuration);
            //var controlNumberExists = payment.CheckControlNumber(model.internal_identifier, model.control_number);

            if (model.control_number != null)
            {
                model.type_of_payment = null;

                var paymentId = payment.GetPaymentId(model.control_number);

                if (paymentId != null && paymentId != string.Empty)
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
                var success = payment.UpdatePaymentTransferFee(payment.PaymentId, transferFee, (TypeOfPayment)payment.typeOfPayment);
                model.received_amount = model.received_amount + Convert.ToDouble(transferFee);
            }
            else if (model.type_of_payment != null && payment.typeOfPayment == null)
            {
                var transferFee = payment.determineTransferFeeReverse(Convert.ToDecimal(model.received_amount), (TypeOfPayment)model.type_of_payment);
                var success = payment.UpdatePaymentTransferFee(payment.PaymentId, transferFee, (TypeOfPayment)model.type_of_payment);
                model.received_amount = model.received_amount + Convert.ToDouble(transferFee);
            }

            var response = payment.SavePayment(model);



            if (payment.PaymentId != null && !response.ErrorOccured)
            {
                var ackResponse = payment.GetPaymentDataAck(payment.PaymentId, payment.ControlNum);

                MatchModel matchModel = new MatchModel() { internal_identifier = payment.PaymentId, audit_user_id = -3 };

                var matchresponse = await MatchPayment(matchModel);

                var matchdata = JsonConvert.SerializeObject(matchresponse.Data);
                var matchedPayments = JsonConvert.DeserializeObject<List<MatchedPayment>>(matchdata);

                if (matchedPayments.Select(x => x.PaymentId).Contains(payment.PaymentId))
                {
                    SendPaymentSms(payment);
                }

            }

            return response;
        }

        public DataMessage SaveControlNumber(ControlNumberResp model)
        {

            PaymentRepository payment = new PaymentRepository(Configuration);
            var controlNumberExists = payment.CheckControlNumber(model.internal_identifier, model.control_number);
            var response = payment.SaveControlNumber(model, controlNumberExists);

            if (payment.PaymentId != null)
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

        public async void ControlNumberAssignedSms(IPaymentRepository payment)
        {
            //Language lang = payment.Language.ToLower() == "en" || payment.Language.ToLower() == "english" || payment.Language.ToLower() == "primary" ? Language.Primary : Language.Secondary;
            ImisSms sms = new ImisSms(Configuration, WebRootPath, ContentRootPath, payment.Language);
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
                DateTime.UtcNow.ToLongDateString(),
                DateTime.UtcNow.ToLongTimeString(),
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
            PaymentRepository payment = new PaymentRepository(Configuration);
            var PaymentIds = requests.requests.Select(x => x.internal_identifier).ToArray();

            var PaymentIds_string = string.Join(",", PaymentIds);

            var response = await payment.GetControlNumbers(PaymentIds_string);

            return response;
        }

        public async void ControlNumberNotassignedSms(IPaymentRepository payment, string error)
        {
            //Language lang = payment.Language.ToLower() == "en" || payment.Language.ToLower() == "english" || payment.Language.ToLower() == "primary" ? Language.Primary : Language.Secondary;
            ImisSms sms = new ImisSms(Configuration, WebRootPath, ContentRootPath, payment.Language);
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
                DateTime.UtcNow.ToLongDateString(),
                DateTime.UtcNow.ToLongTimeString(),
                payment.InsureeProducts.FirstOrDefault().InsureeNumber,
                payment.InsureeProducts.FirstOrDefault().ProductCode,
                error,
                othersCount);

            List<SmsContainer> message = new List<SmsContainer>();
            message.Add(new SmsContainer() { Message = txtmsg, Recipient = payment.PhoneNumber });

            var fileName = "CnError_" + payment.PhoneNumber;

            string test = await sms.SendSMS(message, fileName);
        }

        public async void SendPaymentSms(PaymentRepository payment)
        {
            // Language lang = payment.Language.ToLower() == "en" || payment.Language.ToLower() == "english" || payment.Language.ToLower() == "primary" ? Language.Primary : Language.Secondary;
            ImisSms sms = new ImisSms(Configuration, WebRootPath, ContentRootPath, payment.Language);
            List<SmsContainer> message = new List<SmsContainer>();
            var familyproduct = payment.InsureeProducts.FirstOrDefault();

            if (familyproduct.PolicyActivated)
            {

                var txtmsg = string.Format(sms.GetMessage("PaidAndActivated"),
                    payment.PaidAmount,
                    DateTime.UtcNow.ToLongDateString(),
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
                    DateTime.UtcNow.ToLongDateString(),
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
            payment.MatchedSmsSent();
        }

        public async void SendMatchSms(PaymentRepository payment)
        {
            // Language lang = payment.Language.ToLower() == "en" || payment.Language.ToLower() == "english" || payment.Language.ToLower() == "primary" ? Language.Primary : Language.Secondary;
            ImisSms sms = new ImisSms(Configuration, WebRootPath, ContentRootPath, payment.Language);
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
                     DateTime.UtcNow.ToLongDateString(),
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
                bool shoulSendSms = LocalDefault.ShouldSendSms(Configuration, m.DateLastSms, m.MatchedDate);

                if (shoulSendSms)
                {
                    var txtmsgTemplate = string.Empty;
                    string othersCount = string.Empty;

                    PaymentRepository _pay = new PaymentRepository(Configuration);
                    _pay.GetPaymentInfo(m.PaymentId.ToString());

                    //Language lang = _pay.Language.ToLower() == "en" || _pay.Language.ToLower() == "english" || _pay.Language.ToLower() == "primary" ? Language.Primary : Language.Secondary;
                    ImisSms sms = new ImisSms(Configuration, WebRootPath, ContentRootPath, _pay.Language);

                    if (_pay.PaymentId != null)
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
                                 DateTime.UtcNow.ToLongDateString(),
                                 _pay.ControlNum,
                                 familyproduct.InsureeNumber,
                                 familyproduct.InsureeName,
                                 familyproduct.ProductCode,
                                 familyproduct.ProductName,
                                 othersCount);


                        message.Add(new SmsContainer() { Message = txtmsg, Recipient = _pay.PhoneNumber });
                        _pay.UnMatchedSmsSent(m.PaymentId);
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
    }
}
