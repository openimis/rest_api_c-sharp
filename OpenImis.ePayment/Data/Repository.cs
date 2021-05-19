using OpenImis.ePayment.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Data
{
    public class Repository
    {
        private ImisValidate dh;

        public Repository(IConfiguration configuration)
        {
            dh = new ImisValidate(configuration);
        }

        

    }
}
