using ImisRestApi.Escape.Payment.Models;
using ImisRestApi.Models;
using ImisRestApi.Models.Payment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.ImisAttributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class ReqNoOfficerAttribute : ValidationAttribute
    {
        private string _fieldName;
        private int _fieldNumber;

        public ReqNoOfficerAttribute(string fieldName) // fieldNumber = 1:control_number 2:insurance_number, 3:insurance_product_code, 4:renewal, 5:enrolment_officer_code
        {
            _fieldName = fieldName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IntentOfSinglePay intent = (IntentOfSinglePay)validationContext.ObjectInstance;

            if (intent.OfficerCode == null && value != null && Convert.ToInt32(value) != 0)
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
