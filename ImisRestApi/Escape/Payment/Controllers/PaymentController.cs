using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ImisRestApi.Chanels;
using ImisRestApi.Chanels.Payment.Models;
using ImisRestApi.Data;
using ImisRestApi.Escape.Payment.Models;
using ImisRestApi.Models;
using ImisRestApi.Models.Payment;
using ImisRestApi.Models.Payment.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ImisRestApi.Controllers
{

    public class PaymentController : PaymentBaseController
    {
        private ImisPayment imisPayment;
        private IHostingEnvironment env;

        public PaymentController(IConfiguration configuration, IHostingEnvironment hostingEnvironment) : base(configuration, hostingEnvironment)
        {
            imisPayment = new ImisPayment(configuration, hostingEnvironment);
            env = hostingEnvironment;
        }

#if CHF
        [NonAction]
        public override Task<IActionResult> GetControlNumber([FromBody] IntentOfPay intent)
        {           
            return base.GetControlNumber(intent);
        }

        [HttpPost]
        [Route("api/GetControlNumber")]
        [ProducesResponseType(typeof(GetControlNumberResp), 200)]
        [ProducesResponseType(typeof(ErrorResponseV2), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public  async Task<IActionResult> GetControlNumberV2([FromBody] IntentOfPay intent)
        {

            return await base.GetControlNumber(intent);
        }

        [HttpPost]
        [Route("api/GetControlNumber/Single")]
        public async Task<IActionResult> Index([FromBody]IntentOfSinglePay payment)
        {
            payment.phone_number = payment.Msisdn;
            payment.enrolment_officer_code = payment.OfficerCode;
            payment.SmsRequired = true;

            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { success = false, message = error, error_occured = true, error_message = error });
            }

            try
            {
                if (payment.enrolment_officer_code == null)
                    payment.EnrolmentType = EnrolmentType.Renewal + 1;

                payment.SetDetails();

                var response = await _payment.SaveIntent(payment);
                AssignedControlNumber data = (AssignedControlNumber)response.Data;

                return Ok(new { success = !response.ErrorOccured, message = response.MessageValue, error_occured = response.ErrorOccured, error_message = response.MessageValue, internal_identifier =data.internal_identifier, control_number = data.control_number });

            }
            catch (Exception e)
            {
               
                 return BadRequest(new { success = false, message = e.Message, error_occured = true, error_message = e.Message });
            }   
        }

        [HttpPost]
        [Route("api/GetReconciliationData")]
        public IActionResult GetReconciliation([FromBody] GepgReconcMessage model)
        {
            if (imisPayment.IsValidCall(model, 0))
            {
                if (!ModelState.IsValid)
                    return BadRequest(imisPayment.ReconciliationResp(7101));

                string mydocpath = System.IO.Path.Combine(env.WebRootPath, "Reconciliations");
                string namepart = new Random().Next(100000, 999999).ToString();

                string reconc = JsonConvert.SerializeObject(model);

                using (StreamWriter outputFile = new StreamWriter(Path.Combine(mydocpath, "Reconc_" + namepart + ".json")))
                {
                    outputFile.WriteLine(reconc);
                }

                return Ok(imisPayment.ReconciliationResp(7101));
            }
            else
            {
                string mydocpath = System.IO.Path.Combine(env.WebRootPath, "Reconciliations");
                string namepart = new Random().Next(100000, 999999).ToString();

                string reconc = JsonConvert.SerializeObject(model);

                using (StreamWriter outputFile = new StreamWriter(Path.Combine(mydocpath, "ReconcInvalidSig_" + namepart + ".json")))
                {
                    outputFile.WriteLine(reconc);
                }
                return Ok(imisPayment.ReconciliationResp(7101));
            }
        }

        [HttpGet]
        [Route("api/Reconciliation")]
        public IActionResult Reconciliation(int daysAgo)
        {
            List<object> done = new List<object>();
            // Make loop for all product from database that have account follow SP[0-9]{3} and do the function for all sp codes
            var productsSPCodes = imisPayment.GetProductsSPCode();
            if (productsSPCodes.Count > 0)
            {
                foreach (String productSPCode in productsSPCodes)
                {
                    var result = imisPayment.RequestReconciliationReport(daysAgo, productSPCode);
                    //check if we have done result - if no - then return 500
                    System.Reflection.PropertyInfo pi = result.GetType().GetProperty("resp");
                    done.Add(result);
                }
            }
            else
            {
                //return not found - no sp codes to proceed 
                return NotFound();
            }
            return Ok(done);
        }

        [HttpPost]
        [Route("api/WebMatchPayment")]
        public async Task<IActionResult> WebMatchPayment([FromBody]WebMatchModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error_occured = true, error_message = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage });

            if (model.api_key != "Xdfg8796021ff89Df4654jfjHeHidas987vsdg97e54ggdfHjdt")
                return BadRequest(new { error_occured = true, error_message = "Unauthorized request" });

            try
            {
                MatchModel match = new MatchModel()
                {
                    internal_identifier = model.internal_identifier,
                    audit_user_id = model.audit_user_id
                };

                var response = await base.MatchPayment(match);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest(new { error_occured = true, error_message = "Unknown Error Occured" });
            }

        }

        [NonAction]
        public override IActionResult GetReqControlNumber([FromBody] ControlNumberResp model)
        {
            return base.GetReqControlNumber(model);
        }

        [NonAction]
        public override async Task<IActionResult> GetPaymentData([FromBody] PaymentData model)
        {
            return await base.GetPaymentData(model);
        }

        [NonAction]
        public override IActionResult PostReqControlNumberAck([FromBody] Acknowledgement model)
        {
            return base.PostReqControlNumberAck(model);
        }

        [HttpPost]
        [Route("api/GetPaymentData")]
        public async Task<IActionResult> GetPaymentChf([FromBody] GepgPaymentMessage model)
        {
            if (imisPayment.IsValidCall(model, 0))
            {
                if (!ModelState.IsValid)
                    return BadRequest(imisPayment.PaymentResp(7101));

                object _response = null;

                foreach (var payment in model.PymtTrxInf)
                {
                    
                    PaymentData pay = new PaymentData()
                    {   
                        control_number = payment.PayCtrNum,
                        insurance_product_code = payment.PayRefId,
                        enrolment_officer_code = payment.PyrName,
                        transaction_identification = payment.TrxId,
                        received_amount = Convert.ToDouble(payment.BillAmt),
                        received_date = DateTime.UtcNow.ToString("yyyy/MM/dd"),
                        payment_date = payment.TrxDtTm,
                        payment_origin = "GePG",
                        receipt_identification = payment.PspReceiptNumber,
                        insurance_number = payment.PyrCellNum.ToString()
                    };

                    _response = await base.GetPaymentData(pay);

                }

                string mydocpath = System.IO.Path.Combine(env.WebRootPath, "Payments");
                string namepart = new Random().Next(100000, 999999).ToString();

                string reconc = JsonConvert.SerializeObject(model);
                var payresponse = JsonConvert.SerializeObject(_response);

                using (StreamWriter outputFile = new StreamWriter(Path.Combine(mydocpath, "Payment_" + namepart + ".json")))
                {
                    outputFile.WriteLine(reconc +"________"+ payresponse);
                }

                return Ok(imisPayment.PaymentResp(7101));
            }
            else
            {

                string mydocpath = System.IO.Path.Combine(env.WebRootPath, "Payments");
                string namepart = new Random().Next(100000, 999999).ToString();

                string reconc = JsonConvert.SerializeObject(model);

                using (StreamWriter outputFile = new StreamWriter(Path.Combine(mydocpath, "InvalidSignature" + namepart + ".json")))
                {
                    outputFile.WriteLine(reconc);
                }

                return Ok(imisPayment.PaymentResp(7101));
            }

         }

        [HttpPost]
        [Route("api/GetReqControlNumber")]
        public IActionResult GetReqControlNumberChf([FromBody] GepgBillResponse model)
        {
            if (imisPayment.IsValidCall(model, 0))
            {
                if (!ModelState.IsValid)
                    return BadRequest(imisPayment.ControlNumberResp(7246));

                foreach (var bill in model.BillTrxInf)
                {
                    ControlNumberResp ControlNumberResponse = new ControlNumberResp();

                    if (bill.TrxStsCode == "7101")
                    {
                        ControlNumberResponse = new ControlNumberResp()
                        {
                            internal_identifier = bill.BillId,
                            control_number = bill.PayCntrNum,
                            error_occured = false,
                            error_message = bill.TrxStsCode
                        };

                    }
                    else
                    {
                        ControlNumberResponse = new ControlNumberResp()
                        {
                            internal_identifier = bill.BillId,
                            control_number = bill.PayCntrNum,
                            error_occured = true,
                            error_message = bill.TrxStsCode
                        };
                    }


                    try
                    {
                        var response = base.GetReqControlNumber(ControlNumberResponse);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }

                return Ok(imisPayment.ControlNumberResp(7101));
            }
            else
            {

                string mydocpath = System.IO.Path.Combine(env.WebRootPath, "Reconciliations");
                string namepart = new Random().Next(100000, 999999).ToString();

                string reconc = JsonConvert.SerializeObject(model);

                using (StreamWriter outputFile = new StreamWriter(Path.Combine(mydocpath, "ControlNumberAttempt_" + namepart + ".json")))
                {
                    outputFile.WriteLine(reconc);
                }

                return Ok(imisPayment.ControlNumberResp(7303));
            }

        }

#endif
    }
}