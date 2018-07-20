using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Repo
{
    public class Connection
    {
        private readonly string ConnectionString;


        public Connection(IConfiguration configuration)
        {
            ConnectionString = configuration["ConnectionStrings:DefaultConnection"];
        }
    }
}
