using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using OpenImis.Modules.PaymentModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.PaymentModule.Helpers.SMS
{
    public class ImisSms : ImisBaseSms
    {
        public ImisSms(IConfiguration config, string webRootPath, string contentRootPath, Language language = Language.Primary) : base(config, webRootPath, contentRootPath, language)
        {

        }
    }
}
