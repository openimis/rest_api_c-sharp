using OpenImis.ePayment.ImisAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Models
{
    public class DsiInputModel
    {
        [ValidDate(ErrorMessage = "Please Enter A valid date format")]
        public string last_update_date { get; set; }
    }
}
