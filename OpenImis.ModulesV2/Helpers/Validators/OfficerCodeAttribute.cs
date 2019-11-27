using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenImis.ModulesV2.Helpers.Validators
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class OfficerCodeAttribute : ValidationAttribute
    {
        public OfficerCodeAttribute()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Validation val = new Validation();
            ValidationResult result = val.OfficerCode(value);

            return result;
        }
    }
}
