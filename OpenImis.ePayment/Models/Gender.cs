using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenImis.ePayment.Models
{
    public class GenderValue
    {
        public GenderValue(Gender g)
        {
            switch (g)
            {
                case Gender.Male:
                    Value = "M";
                    break;
                case Gender.Female:
                    Value = "F";
                    break;
                case Gender.Other:
                    Value = "O";
                    break;
                default:
                    Value = "";
                    break;
            }
        }

        public string Value { get; set; }
    }
    public enum Gender
    {
       
        Male,
        Female,
        Other
    }
}