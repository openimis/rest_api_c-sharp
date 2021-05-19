using OpenImis.ePayment.Models;
using OpenImis.ePayment.Models.Payment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.ImisAttributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class RequiredIfEoAttribute : ValidationAttribute
    {
        private string _fieldName;
        private int _fieldNumber;

        public RequiredIfEoAttribute(string fieldName) // fieldNumber = 1:control_number 2:insurance_number, 3:insurance_product_code, 4:renewal, 5:enrolment_officer_code
        {
            _fieldName = fieldName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IntentOfPay intent = (IntentOfPay)validationContext.ObjectInstance;

            if (intent.enrolment_officer_code == null && value != null && Convert.ToInt32(value) != 0)
            {
                return new ValidationResult(_fieldName + " is not required if Enrolment officer is not provided");
            }
            else
            {
                return ValidationResult.Success;
            }

        }
    }
}
