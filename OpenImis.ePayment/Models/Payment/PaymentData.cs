using OpenImis.ePayment.ImisAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Models.Payment
{
    public class PaymentData
    {
        public string control_number { get; set; }
        [InsureeNumber]
        [RequiredIf("Insuree number")] //this attributes validates if control number is not provided
        public string insurance_number { get; set; }
        [RequiredIf("Insurance Product Code")]
        public string insurance_product_code { get; set; }     
        [RequiredIf("Renewal")]
        public EnrolmentType? renewal { get; set; }
        [RequiredIf("Enrolment Officer Code",2)]
        public string enrolment_officer_code { get; set; }
        public string transaction_identification { get; set; }
        public string receipt_identification { get; set; }
        public double received_amount { get; set; }
        [Required(ErrorMessage = "1-Wrong or missing receiving date")]
        [ValidDate(ErrorMessage = "1-Wrong or missing receiving date")]
        [DataType(DataType.DateTime)]
        public string received_date { get; set; }
        [ValidDate(ErrorMessage = "5-Invalid Payment Date")]
        [DataType(DataType.DateTime)]
        public string payment_date { get; set; }
        public string payment_origin { get; set; }
        public string language { get; set; }
        [Range(0, 2, ErrorMessage = "10-Uknown type of payment")]
        public TypeOfPayment? type_of_payment { get; set; }
    }
}
