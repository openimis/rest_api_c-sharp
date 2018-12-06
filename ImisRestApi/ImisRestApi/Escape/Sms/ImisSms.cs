using ImisRestApi.Data;
using ImisRestApi.Models.Sms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ImisRestApi.Chanels.Sms
{
    public class ImisSms:ImisBaseSms
    {    
        public ImisSms(IConfiguration config, IHostingEnvironment env) :base(config,env)
        {
          
        }
    }
}
