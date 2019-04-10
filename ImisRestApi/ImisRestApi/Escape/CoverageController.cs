using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImisRestApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ImisRestApi.Controllers
{
    public class CoverageController : CoverageBaseController
    {
        public CoverageController(IConfiguration configuration) : base(configuration)
        {

        }
       
    }
}