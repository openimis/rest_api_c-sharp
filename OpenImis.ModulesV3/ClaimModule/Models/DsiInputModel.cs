
using OpenImis.ModulesV3.Helpers.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ModulesV3.ClaimModule.Models
{
    public class DsiInputModel
    {
        [ValidDate(ErrorMessage = "Please Enter A valid date format")]
        public string last_update_date { get; set; }
    }
}
