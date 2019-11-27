using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenImis.ModulesV2.PaymentModule.Helpers.SMS;
using OpenImis.ModulesV2.PaymentModule.Models;
using OpenImis.ModulesV2.PaymentModule.Models.Response;
using OpenImis.ModulesV2.PaymentModule.Models.SMS;
using OpenImis.ModulesV2.PaymentModule.Repositories;
using OpenImis.ModulesV2.PolicyModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ModulesV2.PaymentModule.Logic
{
    public class PaymentLogic : IPaymentLogic
    {
        private IConfiguration _configuration;

        public string WebRootPath { get; set; }
        public string ContentRootPath { get; set; }

        public PaymentLogic(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<DataMessage> SaveIntent(IntentOfPay intent, int? errorNumber = 0, string errorMessage = null)
        {
            IPaymentRepository payment = new PaymentRepository(_configuration);
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

        public async void ControlNumberAssignedSms(IPaymentRepository payment)
        {
            ImisSms sms = new ImisSms(_configuration, WebRootPath, ContentRootPath, payment.Language);
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

        public async void ControlNumberNotassignedSms(IPaymentRepository payment, string error)
        {
            ImisSms sms = new ImisSms(_configuration, WebRootPath, ContentRootPath, payment.Language);
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

        public async Task<DataMessage> GetControlNumbers(PaymentRequest requests)
        {
            PaymentRepository payment = new PaymentRepository(_configuration);
            var PaymentIds = requests.requests.Select(x => x.internal_identifier).ToArray();

            var PaymentIds_string = string.Join(",", PaymentIds);

            var response = await payment.GetControlNumbers(PaymentIds_string);

            return response;
        }
    }
}
