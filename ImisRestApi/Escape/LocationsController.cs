using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImisRestApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ImisRestApi.Escape
{

    public class LocationsController : LocationsBaseController
    {
        public LocationsController(IConfiguration configuration) : base(configuration)
        {

        }
    }
}