using ImisRestApi.ImisAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace ImisRestApi.Models
{
    public class PaymentContainer
    {
        [Required]
        public int PaymentId { get; set; }
        public string ControlNumber { get; set; }
        [InsureeNumber]
        public string InsureeNumber { get; set; }
        [Required]
        public string ProductCode { get; set; }
        public string EnrolmentOfficerCode { get; set; }
        public string TransactionId { get; set; }
        [Required]
        public decimal ReceivedAmount { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentOrigin { get; set; }
    }
}