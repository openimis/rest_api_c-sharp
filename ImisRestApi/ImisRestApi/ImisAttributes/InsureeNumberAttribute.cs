using ImisRestApi.Escape;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.ImisAttributes
{
   
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class InsureeNumberAttribute : ValidationAttribute
    {
        
        public InsureeNumberAttribute()
        {
            
        }
         
        
        protected override ValidationResult IsValid(object value,ValidationContext validationContext)
        {
            ValidationResult result = Validation.InsureeNumber(value.ToString());

            return result;
        }

       
    }
}
