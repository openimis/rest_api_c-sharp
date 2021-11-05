using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenImis.ePayment.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace OpenImis.ePayment.Escape
{
    [ApiVersion("3")]
    public class LocationsController : LocationsBaseController
    {
        public LocationsController(IConfiguration configuration) : base(configuration)
        {

        }
    }
}