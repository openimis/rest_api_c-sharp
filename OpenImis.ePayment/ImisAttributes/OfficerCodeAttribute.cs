using OpenImis.ePayment.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.ImisAttributes
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
