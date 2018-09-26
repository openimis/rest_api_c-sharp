using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Logic
{
    public static class LocalDefault
    {
        public static object FamilyMambers(IConfiguration config)
        {
            var adults = config["DefaultFamily:Adults"];
            var children = config["DefaultFamily:Children"];

            return new { Adults = adults, Children = children };
        }
    }
}
