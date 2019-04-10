using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ImisRestApi.Controllers
{
    public class PoliciesController : PoliciesBaseController
    {
        public PoliciesController(IConfiguration configuration) : base(configuration)
        {

        }
    }
}