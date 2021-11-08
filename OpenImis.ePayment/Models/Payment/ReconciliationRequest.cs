using System;
using System.ComponentModel.DataAnnotations;

namespace OpenImis.ePayment.Models.Payment
{
    public class ReconciliationRequest
    {
        [Required]
        [DataType(DataType.DateTime)]
        public string date_from { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public string date_to { get; set; }
    }
}
