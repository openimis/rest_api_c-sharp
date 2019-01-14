﻿using ImisRestApi.Chanels.Payment.Models;
using ImisRestApi.ImisAttributes;
using System.ComponentModel.DataAnnotations;

namespace ImisRestApi.Models
{
    public class PaymentDetail
    {
        [InsureeNumber]
        public string insuree_number { get; set; }
        [Required(ErrorMessage = "2:Not valid insurance or missing product code")]
        public string insuree_product_code { get; set; }
        [Required(ErrorMessage = "10:EnrolmentType was not provided")]
        [Range(0,2)]
        public EnrolmentType? renewal { get; set; }
        public int IsRenewal()
        {
            switch (renewal)
            {
                case EnrolmentType.Renewal:
                    return 1;
                case EnrolmentType.New:
                    return 0;
                default:
                    return 0;
            }
        }
    }
}