using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ePayment.Models.Payment
{
    public class OfficerDetailsVM
    {
        public int OfficerId { get; set; }
        public string Code { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string Phone { get; set; }
        public string EmailId { get; set; }
    }
}
