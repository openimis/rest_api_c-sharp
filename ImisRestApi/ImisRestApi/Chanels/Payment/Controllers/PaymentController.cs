using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImisRestApi.Chanels;
using ImisRestApi.Chanels.Payment.Models;
using ImisRestApi.Models.Payment;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ImisRestApi.Controllers
{
    public class PaymentController : PaymentBaseController
    {
        public PaymentController(IConfiguration configuration, IHostingEnvironment hostingEnvironment) :base(configuration, hostingEnvironment)
        {

        }

        [HttpPost]
        [Route("api/GetControlNumber/Single")]
        public IActionResult Index([FromBody]IntentOfSinglePay payment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _payment.SaveIntent(payment);

            return Ok();
        }

        [HttpPost]
        [Route("api/GetReconciliationData")]
        public IActionResult GetReconciliation([FromBody]GepgReconcMessage model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok();
        }

        public override IActionResult ReceiveControlNumber([FromBody] GepgBillResponse model)
        {
            return base.ReceiveControlNumber(model);
        }

        public override IActionResult GetPayment([FromBody] PaymentData model)
        {
            return base.GetPayment(model);
        }
    }
}