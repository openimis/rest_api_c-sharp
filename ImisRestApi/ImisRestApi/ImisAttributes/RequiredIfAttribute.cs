using ImisRestApi.Models.Payment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.ImisAttributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class RequiredIfAttribute : ValidationAttribute
    {
        private string _fieldName;
        public RequiredIfAttribute(string fieldName)
        {
            _fieldName = fieldName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PaymentData payment = (PaymentData)validationContext.ObjectInstance;

            if (payment.InternalIdentifier == null && value == null)
            {
                return new ValidationResult(_fieldName + " is required if PaymentId is not provided");
            }
            else
            {
                return ValidationResult.Success;
            }

        }
    }
}
