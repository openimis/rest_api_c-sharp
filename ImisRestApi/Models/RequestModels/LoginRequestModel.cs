using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models.RequestModels
{
    /// <summary>
    /// This class serves as a parameter class for the Login call 
    /// </summary>
    public class LoginRequestModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
