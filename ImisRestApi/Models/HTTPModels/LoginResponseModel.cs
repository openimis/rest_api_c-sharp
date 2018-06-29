using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models.HTTPModels
{
    /// <summary>
    /// This class serves as a parameter class for the Login call 
    /// </summary>
    public class LoginResponseModel
    {
        public string Token { get; set; }

        public DateTime Expires { get; set; }
    }
}
