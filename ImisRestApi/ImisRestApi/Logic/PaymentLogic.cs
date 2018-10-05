using ImisRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImisRestApi.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using ImisRestApi.Models.Payment;
using ImisRestApi.Responses;
using ImisRestApi.Models.Sms;
using ImisRestApi.Chanels.Sms;

namespace ImisRestApi.Logic
{
    public class PaymentLogic
    {

        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PaymentLogic(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
           
        }
        public async Task<DataMessage> SaveIntent(IntentOfPay intent,int? errorNumber = 0,string errorMessage = null)
        {

            ImisPayment payment = new ImisPayment(_configuration, _hostingEnvironment);
            var intentResponse = payment.SaveIntent(intent,errorNumber,errorMessage);

            string url = _configuration["PaymentGateWay:Url"] + _configuration["PaymentGateWay:CNRequest"];

            DataMessage return_message = new DataMessage();

            if (intentResponse.Code == 0)
            {
                var response = payment.GenerateCtrlNoRequest(intent.OfficerCode, payment.PaymentId, payment.ExpectedAmount, intent.PaymentDetails);
              
                if (response.ControlNumber != null)
                {
                    var controlNumberExists = payment.CheckControlNumber(payment.PaymentId,response.ControlNumber);
                    return_message = payment.SaveControlNumber(response.ControlNumber,controlNumberExists);

                    if (!return_message.ErrorOccured && controlNumberExists) {
                        ControlNumberAssignedSms(payment);
                    }
                    else
                    {
                        ControlNumberNotassignedSms(payment,return_message.MessageValue);
                    }
                }
                else if (response.RequestAcknowledged == true)
                {
                    return_message = payment.SaveControlNumberAkn(!response.ErrorOccured,response.ErrorMessage);
                }
                else if (response.ErrorOccured == true)
                {
                    return_message = payment.SaveControlNumberAkn(!response.ErrorOccured,response.ErrorMessage);
                    ControlNumberNotassignedSms(payment,response.ErrorMessage);
                   
                }
                else
                {

                }
            }
            else
            {
                var response = payment.GenerateCtrlNoRequest(intent.OfficerCode, payment.PaymentId, payment.ExpectedAmount, intent.PaymentDetails,null,false,true);
                return_message = intentResponse;
            }
            
            return return_message;
        }

        public async Task<DataMessage> Match(MatchModel model)
        {
            ImisPayment payment = new ImisPayment(_configuration, _hostingEnvironment);
            var response = payment.Match(model);

            SendMatchSms(payment);

            return response;
        }

        

        public DataMessage SaveAcknowledgement(Acknowledgement model)
        {
            ImisPayment payment = new ImisPayment(_configuration, _hostingEnvironment) { PaymentId = model.PaymentId.ToString()};
            var response = payment.SaveControlNumberAkn(model.Success, model.Description);

            return response;
        }

        public DataMessage SavePayment(PaymentData model)
        {
            
            ImisPayment payment = new ImisPayment(_configuration, _hostingEnvironment);
            var controlNumberExists = payment.CheckControlNumber(model.PaymentId, model.ControlNumber);
            var response = payment.SavePayment(model,controlNumberExists);

            if(model.EnrolmentOfficerCode == null)
            {
                SendPaymentSms(payment);
            }

            return response;
        }      

        public DataMessage SaveControlNumber(ControlNumberResp model)
        {
            ImisPayment payment = new ImisPayment(_configuration, _hostingEnvironment);
            var controlNumberExists = payment.CheckControlNumber(model.PaymentId, model.ControlNumber);
            var response = payment.SaveControlNumber(model,controlNumberExists);

            if (!response.ErrorOccured && controlNumberExists)
            {
                ControlNumberAssignedSms(payment);
            }
            else
            {
                ControlNumberNotassignedSms(payment, response.MessageValue);
            }

            return response;
        }

        public async void ControlNumberAssignedSms(ImisPayment payment)
        {

            ImisSms sms = new ImisSms(_configuration, _hostingEnvironment);
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
            var txtmsg = string.Format(txtmsgTemplate,
                payment.ControlNum,
                DateTime.UtcNow.ToLongDateString(),
                DateTime.UtcNow.ToLongTimeString(),
                payment.InsureeProducts.FirstOrDefault().InsureeNumber,
                payment.InsureeProducts.FirstOrDefault().InsureeName,
                payment.InsureeProducts.FirstOrDefault().ProductCode,
                payment.InsureeProducts.FirstOrDefault().ProductName,
                payment.ExpectedAmount,
                othersCount);

            List<SmsContainer> message = new List<SmsContainer>();
            message.Add(new SmsContainer() { Message = txtmsg, Recepient = payment.PhoneNumber });

            string test = await sms.PushSMS(message);
        }

        public async void ControlNumberNotassignedSms(ImisPayment payment,string error)
        {
            ImisSms sms = new ImisSms(_configuration, _hostingEnvironment);
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
                error);

            List<SmsContainer> message = new List<SmsContainer>();
            message.Add(new SmsContainer() { Message = txtmsg, Recepient = payment.PhoneNumber });

            string test = await sms.PushSMS(message);
        }

        public async void SendPaymentSms(ImisPayment payment)
        {
            ImisSms sms = new ImisSms(_configuration, _hostingEnvironment);
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
                    familyproduct.EffectiveDate,
                    familyproduct.ExpiryDate,
                    payment.PaidAmount);

                
                message.Add(new SmsContainer() { Message = txtmsg, Recepient = payment.PhoneNumber });

            }
            else
            {

                var txtmsg = string.Format(sms.GetMessage("PaidAndNotActivated"),
                    payment.PaidAmount,
                    DateTime.UtcNow.ToLongDateString(),
                    payment.ControlNum,
                    familyproduct.InsureeNumber,
                    familyproduct.InsureeName,
                    familyproduct.ProductCode,
                    familyproduct.ProductName,
                    payment.ExpectedAmount,
                    payment.ExpectedAmount - payment.PaidAmount);

                message.Add(new SmsContainer() { Message = txtmsg, Recepient = payment.PhoneNumber });

            }

            string test = await sms.PushSMS(message);
        }

        public async void SendMatchSms(ImisPayment payment)
        {
            ImisSms sms = new ImisSms(_configuration, _hostingEnvironment);
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
                     payment.PaymentId,
                     DateTime.UtcNow.ToLongDateString(),
                     payment.ControlNum,
                     familyproduct.InsureeNumber,
                     familyproduct.InsureeName,
                     familyproduct.ProductCode,
                     familyproduct.ProductName,
                     othersCount);


            message.Add(new SmsContainer() { Message = txtmsg, Recepient = payment.PhoneNumber });
            string test = await sms.PushSMS(message);
        }
    }
}
