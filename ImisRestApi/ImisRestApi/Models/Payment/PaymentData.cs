using ImisRestApi.ImisAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models.Payment
{
    public class PaymentData
    {
        [Required]
        public string PaymentId { get; set; }
        public string ControlNumber { get; set; }
        public string ProductCode { get; set; }
        public string EnrolmentOfficerCode { get; set; }
        public string TransactionId { get; set; }
        public double ReceivedAmount { get; set; }
        [Required(ErrorMessage = "1-Wrong or missing receiving date")]
        public DateTime ReceivedDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentOrigin { get; set; }
        public bool ErrorOccured { get; set; }
        public string ReceiptNumber { get; set; }
        public string PhoneNumber { get; set; }
        [InsureeNumber]
        public string InsureeNumber { get; set; }
        public EnrolmentType PaymentType { get; set; }
    }
}
