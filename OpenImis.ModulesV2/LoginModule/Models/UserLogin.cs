using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenImis.ModulesV2.LoginModule.Models
{
    public class UserLogin
    {
        [Required]
        public string UserID { get; set; }
        [Required]
        [MaxLength(15)]
        public string Password { get; set; }
    }
}
