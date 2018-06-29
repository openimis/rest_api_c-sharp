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
    public class LoginRequestModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

    }
}
