using System;
using System.ComponentModel.DataAnnotations;

namespace OpenImis.Modules.Helpers.Validators
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class RequiredIfAttribute : ValidationAttribute
    {
        private string _fieldName;
        private int _fieldNumber;

        public RequiredIfAttribute(string fieldName, int fieldNumber = 0) // fieldNumber = 1:control_number 2:insurance_number, 3:insurance_product_code, 4:renewal, 5:enrolment_officer_code
        {
            _fieldName = fieldName;
            _fieldNumber = fieldNumber;
        }

        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    PaymentData payment = (PaymentData)validationContext.ObjectInstance;

        //    if (payment.control_number == null && value == null)
        //    {
        //        if (_fieldNumber == 2 && payment.insurance_number != null)
        //        {
        //            return ValidationResult.Success;
        //        }
        //        else
        //        {
        //            return new ValidationResult(_fieldName + " is required if Control Number is not provided");
        //        }
        //    }
        //    if (payment.control_number != null && value != null)
        //    {
        //        return new ValidationResult(_fieldName + " is not required if Control Number is provided");
        //    }
        //    else
        //    {
        //        return ValidationResult.Success;
        //    }
        //}
    }
}
