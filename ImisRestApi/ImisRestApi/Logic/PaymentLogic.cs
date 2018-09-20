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
        public async Task<DataMessage> SaveIntent(IntentOfPay intent)
        {

            ImisPayment payment = new ImisPayment(_configuration, _hostingEnvironment);
            payment.SaveIntent(intent);

            string url = _configuration["PaymentGateWay:Url"] + _configuration["PaymentGateWay:CNRequest"];

           
            var response = payment.GenerateCtrlNoRequest(intent.OfficerCode, payment.PaymentId, payment.ExpectedAmount,intent.PaymentDetails);

            DataMessage return_message = new DataMessage();

            if (response.ControlNumber != null)
            {
                return_message = payment.SaveControlNumber(response.ControlNumber);

            }
            else if (response.ControlNumber == null)
            {
                return_message = payment.SaveControlNumber();
            }
            else if (response.ErrorOccured == true)
            {
                return_message = payment.SaveControlNumberAkn(response.ErrorOccured, "");
            }
            else
            {

            }

            List<SmsContainer> message = new List<SmsContainer>();
            message.Add(new SmsContainer() { Message = "Your Request for control number was Sent", Recepients = "+255767057265" });

            ImisSms sms = new ImisSms(_configuration);
            string test = await sms.PushSMS(message);
            
            return return_message;
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
            var response = payment.SavePayment(model);

            return response;
        }

        public DataMessage SaveControlNumber(ControlNumberResp model)
        {
            ImisPayment payment = new ImisPayment(_configuration, _hostingEnvironment);
            var response = payment.SaveControlNumber(model);

            return response;
        }
    }
}
