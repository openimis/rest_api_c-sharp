using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        public IList<SqlParameter> Procedure(string StoredProcedure, SqlParameter[] parameters)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand();


            command.CommandText = StoredProcedure;
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = sqlConnection;


            if (parameters.Length > 0)
                command.Parameters.AddRange(parameters);

            sqlConnection.Open();

            command.ExecuteNonQuery();

            var rv = parameters.Where(x => x.Direction.Equals(ParameterDirection.Output));
 
            sqlConnection.Close();

            return rv.ToList();
        }
    }
}
