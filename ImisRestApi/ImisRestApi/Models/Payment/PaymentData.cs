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
        public string InternalIdentifier { get; set; }
        public string ControlNumber { get; set; }
        [RequiredIf("Product Code")]
        public string ProductCode { get; set; }
        public string EnrolmentOfficerCode { get; set; }
        public string TransactionId { get; set; }
        public double ReceivedAmount { get; set; }
        [Required(ErrorMessage = "1-Wrong or missing receiving date")]
        public DateTime ReceivedDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentOrigin { get; set; }
        public string ReceiptNumber { get; set; }
        public string PhoneNumber { get; set; }
        [InsureeNumber]
        [RequiredIf("Insuree number")]
        public string InsureeNumber { get; set; }
        [RequiredIf("PaymentType")]
        public EnrolmentType? Renewal { get; set; }

    }
}
