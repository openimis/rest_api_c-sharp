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

        public int Procedure(string StoredProcedure, SqlParameter[] parameters)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand();

            SqlParameter returnParameter = new SqlParameter("@RV", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            command.CommandText = StoredProcedure;
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = sqlConnection;
            command.Parameters.Add(returnParameter);
            if (parameters.Length > 0)
                command.Parameters.AddRange(parameters);

            sqlConnection.Open();

            command.ExecuteNonQuery();

            int rv = int.Parse(returnParameter.Value.ToString());
 
            sqlConnection.Close();

            return rv;
        }
    }
}
