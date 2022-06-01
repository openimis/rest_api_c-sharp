using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Repo
{
    public class Connection
    {
        private readonly string ConnectionString;


        public Connection(IConfiguration configuration)
        {
            ConnectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public IList<SqlParameter> Procedure(string StoredProcedure, SqlParameter[] parameters)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(StoredProcedure, sqlConnection) { CommandType = CommandType.StoredProcedure })
            {

                if (parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                sqlConnection.Open();

                command.ExecuteNonQuery();

                var rv = parameters.Where(x => x.Direction.Equals(ParameterDirection.Output));

                return rv.ToList();
            }
        }
    }
}
