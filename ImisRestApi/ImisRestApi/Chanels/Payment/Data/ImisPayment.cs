using ImisRestApi.Models.Payment;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Data
{
    public class ImisPayment:ImisBasePayment
    {
        public ImisPayment(IConfiguration configuration, IHostingEnvironment hostingEnvironment):base(configuration, hostingEnvironment)
        {

        }

       
    }
}
