using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenImis.Modules.Helpers.Validators
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class InsureeNumberAttribute : ValidationAttribute
    {
        public InsureeNumberAttribute()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                Validation val = new Validation();
                ValidationResult result = val.InsureeNumber(value.ToString());

                return result;
            }

            return ValidationResult.Success;
        }
    }
}
