using ImisRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImisRestApi.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

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
        public bool SaveIntent(IntentOfPay intent)
        {

            //save the intent of pay
            ImisPayment payment = new ImisPayment(_configuration, _hostingEnvironment);
            payment.SaveIntent(intent);

            string url = _configuration["PaymentGateWay:Url"] + _configuration["PaymentGateWay:CNRequest"];

           
            payment.GenerateCtrlNoRequest(intent.OfficerCode, payment.PaymentId, payment.ExpectedAmount,intent.PaymentDetails);

            //ControlNumberRequest response = ControlNumberChanel.PostRequest(url, _paymentRepo.PaymentId, _paymentRepo.ExpectedAmount);

            //if (response.ControlNumber != null)
            //{
            //    _paymentRepo.SaveControlNumber(response.ControlNumber);

            //}
            //else if (response.ControlNumber == null)
            //{
            //    _paymentRepo.SaveControlNumber();
            //}
            //else if (response.RequestAcknowledged)
            //{
            //    _paymentRepo.SaveControlNumberAkn(response.RequestAcknowledged,"");
            //}

           // string test = await Message.PushSMS("Your Request for control number was Sent", "+255767057265");

          //  return Json(new { status = true, sms_reply = true, sms_text = "Your Request for control number was Sent" });
            return true;
        }

        public String ReceiveControlNumber(){
            return "0";
        }


    }
}
