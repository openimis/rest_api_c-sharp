using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenImis.DB.SqlServer;
using OpenImis.DB.SqlServer.DataHelper;
using OpenImis.Modules.CoverageModule.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace OpenImis.Modules.CoverageModule.Repositories
{
    public class CoverageRepository : ICoverageRepository
    {
        private IConfiguration Configuration;

        public CoverageRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public CoverageModel Get(string insureeNumber)
        {
            CoverageModel response = null;
            DataTable data;

            DataHelper helper = new DataHelper(Configuration);

            SqlParameter[] sqlParameters = {
                new SqlParameter("@InsureeNumber", insureeNumber),
            };

            try
            {
                data = helper.GetDataTable("uspAPIGetCoverage", sqlParameters, CommandType.StoredProcedure);
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new Exception();
            }

            if (data.Rows.Count > 0)
            {
                var firstRow = data.Rows[0];
                var jsonString = JsonConvert.SerializeObject(data);
                var coverage_products = JsonConvert.DeserializeObject<List<CoverageProduct>>(jsonString);

                response = new CoverageModel() { OtherNames = firstRow["OtherNames"].ToString(), LastNames = firstRow["LastName"].ToString(), BirthDate = firstRow["DOB"].ToString(), CoverageProducts = coverage_products };
            }

            return response;
        }
    }
}
