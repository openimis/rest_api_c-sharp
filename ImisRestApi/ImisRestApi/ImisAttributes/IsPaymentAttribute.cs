using ImisRestApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.ImisAttributes
{
    /// <summary>
    ///      This Attribute validates if Insuree can Pay for this product
    /// </summary>
    public class IsPaymentAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PaymentDetail detail = (PaymentDetail)value;


            return base.IsValid(value, validationContext);
        }
    }
}
