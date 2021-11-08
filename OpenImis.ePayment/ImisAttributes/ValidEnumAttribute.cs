using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.ImisAttributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class ValidEnumAttribute : ValidationAttribute
    {
     
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Type _type = value.GetType();
            try
            {
                Convert.ChangeType(value,_type);
                return ValidationResult.Success;
            }
            catch (Exception)
            {

                return new ValidationResult("Error");
            }


        }
    }

}
