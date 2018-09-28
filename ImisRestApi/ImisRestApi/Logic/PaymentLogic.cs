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
                }
                else if (response.RequestAcknowledged == true)
                {
                    return_message = payment.SaveControlNumberAkn(!response.ErrorOccured,response.ErrorMessage);
                }
                else if (response.ErrorOccured == true)
                {
                    return_message = payment.SaveControlNumberAkn(!response.ErrorOccured,response.ErrorMessage);
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

            List<SmsContainer> message = new List<SmsContainer>();
            message.Add(new SmsContainer() { Message = "Your Request for control number was Sent", Recepients = "+255767057265" });

            ImisSms sms = new ImisSms(_configuration);
            string test = await sms.PushSMS(message);
            
            return return_message;
        }

        public async Task<DataMessage> Match(MatchModel model)
        {
            ImisPayment payment = new ImisPayment(_configuration, _hostingEnvironment);
            var response = payment.Match(model);

            List<SmsContainer> message = new List<SmsContainer>();
            message.Add(new SmsContainer() { Message = "Your Payment has been Matched", Recepients = "+255767057265" });

            ImisSms sms = new ImisSms(_configuration);
            string test = await sms.PushSMS(message);

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

            return response;
        }

        public DataMessage SaveControlNumber(ControlNumberResp model)
        {
            ImisPayment payment = new ImisPayment(_configuration, _hostingEnvironment);
            var controlNumberExists = payment.CheckControlNumber(model.PaymentId, model.ControlNumber);
            var response = payment.SaveControlNumber(model,controlNumberExists);

            return response;
        }
    }
}
