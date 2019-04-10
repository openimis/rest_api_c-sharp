using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImisRestApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ImisRestApi.Escape
{
    public class ContributionsController : ContributionsBaseController
    {
        public ContributionsController(IConfiguration configuration) : base(configuration)
        {

        }
    }
}