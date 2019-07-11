using OpenImis.Modules.Helpers.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.ClaimModule.Models
{
    public class DsiInputModel
    {
        [ValidDate(ErrorMessage = "Please Enter A valid date format")]
        public string last_update_date { get; set; }
    }
}
