using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenImis.ePayment.Models.Payment
{
    public class PaymentCancelBaseModel
    {
        public int payment_id;
    }

    public class PaymentCancelModel: PaymentCancelBaseModel
    {
        [Required]
        public string control_number;
    }
}
