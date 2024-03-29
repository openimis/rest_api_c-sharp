﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OpenImis.ePayment.Data
{
    public class ImisCoverage
    {
        private IConfiguration Configuration;
        public int UserId { get; set; }

        public ImisCoverage(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public DataTable Get(string insureeNumber) {

            DataHelper helper = new DataHelper(Configuration);

            SqlParameter[] sqlParameters = {
                new SqlParameter("@InsureeNumber", insureeNumber),
            };
            
            return helper.GetDataTable("uspAPIGetCoverage", sqlParameters, CommandType.StoredProcedure);
        }
    }
}