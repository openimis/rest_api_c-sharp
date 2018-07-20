using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ImisRestApi.Models;
using Microsoft.Extensions.Configuration;

namespace ImisRestApi.Repo
{
    public class PaymentRepo : Connection
    {
        public int PaymentId { get; set; }
        private IntentOfPay _intent { get; set; }

        public PaymentRepo(IConfiguration configuration,IntentOfPay Intent) : base(configuration)
        {
            _intent = Intent;
        }

        public void SaveIntent()
        {
            using (SqlConnection connection = new SqlConnection())
            {

            }
        }

        public bool Valid(string InsureeNumber,string ProductCode)
        {
            return false;
        }
    }
}
