using OpenImis.ePayment.Models;
using OpenImis.ePayment.Repo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OpenImis.ePayment.ImisAttributes
{
    /// <summary>
    ///      This Attribute validates if Insuree can Pay for this product
    /// </summary>
    public class IsPaymentAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            List<PaymentDetail> details = (List<PaymentDetail>)value;

            XElement xmlPayments = new XElement("Payments",
                                                from d in details select
                                                new XElement("Payment",
                                                    new XElement("InsureeNumber",d.insurance_number),
                                                    new XElement("ProductCode",d.insurance_product_code)
                                                ));
           //    PaymentRepo paymentCheck = new PaymentRepo();

            return ValidationResult.Success;
        }
    }
}
