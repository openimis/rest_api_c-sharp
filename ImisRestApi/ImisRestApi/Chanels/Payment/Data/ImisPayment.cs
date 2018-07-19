using ImisRestApi.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Chanels.Payment.Data
{
    public class ImisPayment
    {
        private IConfiguration Configuration;

        public ImisPayment(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        
        //public string Insert()
        //{

        //    private DataHelper dh = new DataHelper();
        //    return "Ok";
        //}
    }
}
