using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenImis.DB.SqlServer.DataHelper
{
    public class DataHelper
    {
        private readonly string ConnectionString;

        public int ReturnValue { get; set; }

        public DataHelper(IConfiguration configuration)
        {
            ConnectionString = configuration["ConnectionStrings:IMISDatabase"];
        }

        public DataSet FillDataSet(string SQL, SqlParameter[] parameters, CommandType commandType)
        {
            DataSet ds = new DataSet();
            var sqlConnection = new SqlConnection(ConnectionString);

            SqlParameter returnParameter = new SqlParameter("@RV", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            var command = new SqlCommand(SQL, sqlConnection)
            {
                CommandType = commandType
            };

            command.Parameters.Add(returnParameter);

            var adapter = new SqlDataAdapter(command);
            using (command)
            {
                if (parameters.Length > 0)
                    command.Parameters.AddRange(parameters);
                adapter.Fill(ds);
            }

            ReturnValue = int.Parse(returnParameter.Value.ToString());

            return ds;
        }

        public DataTable GetDataTable(string SQL, SqlParameter[] parameters, CommandType commandType)
        {
            DataTable dt = new DataTable();
            var sqlConnection = new SqlConnection(ConnectionString);
            var command = new SqlCommand(SQL, sqlConnection)
            {
                CommandType = commandType
            };

            var adapter = new SqlDataAdapter(command);

            using (command)
            {
                if (parameters.Length > 0)
                    command.Parameters.AddRange(parameters);
                adapter.Fill(dt);
            }

            return dt;
        }

        public void Execute(string SQL, SqlParameter[] parameters, CommandType commandType)
        {
            var sqlConnection = new SqlConnection(ConnectionString);

            //if(SqlCommand.C)
            // sqlConnection.Open
            var command = new SqlCommand(SQL, sqlConnection)
            {
                CommandType = commandType
            };

            using (command)
            {
                if (command.Connection.State == 0)
                {
                    command.Connection.Open();

                    if (parameters.Length > 0)
                        command.Parameters.AddRange(parameters);

                    command.ExecuteNonQuery();

                    command.Connection.Close();
                }
            }
        }

        public ProcedureOutPut Procedure(string StoredProcedure, SqlParameter[] parameters, int tableIndex = 0)
        {
            DataSet dt = new DataSet();

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand();
            //SqlDataReader reader;

            SqlParameter returnParameter = new SqlParameter("@RV", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            command.CommandText = StoredProcedure;
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = sqlConnection;
            command.Parameters.Add(returnParameter);

            if (parameters.Length > 0)
                command.Parameters.AddRange(parameters);

            var adapter = new SqlDataAdapter(command);

            using (command)
            {
                adapter.Fill(dt);
            }

            int rv = int.Parse(returnParameter.Value.ToString());
            DataTable dat = new DataTable();

            if (rv == 0)
            {
                dat = dt.Tables[tableIndex];
            }

            var output = new ProcedureOutPut
            {
                Code = rv,
                Data = dat
            };

            return output;
        }

        public IList<SqlParameter> ExecProcedure(string StoredProcedure, SqlParameter[] parameters)
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

            var rv = parameters.Where(x => x.Direction.Equals(ParameterDirection.Output) || x.Direction.Equals(ParameterDirection.ReturnValue)).ToList();
            rv.Add(returnParameter);

            sqlConnection.Close();

            return rv;
        }

        public DataSet GetDataSet(string SQL, SqlParameter[] parameters, CommandType commandType)
        {
            DataSet ds = new DataSet();
            var sqlConnection = new SqlConnection(ConnectionString);
            var command = new SqlCommand(SQL, sqlConnection)
            {
                CommandType = commandType
            };

            var adapter = new SqlDataAdapter(command);

            using (command)
            {
                if (parameters.Length > 0)
                    command.Parameters.AddRange(parameters);
                adapter.Fill(ds);
            }

            return ds;
        }

        public async Task ExecuteAsync(string SQL, SqlParameter[] parameters, CommandType commandType)
        {
            var sqlConnection = new SqlConnection(ConnectionString);

            //if(SqlCommand.C)
            // sqlConnection.Open
            var command = new SqlCommand(SQL, sqlConnection)
            {
                CommandType = commandType
            };

            using (command)
            {
                if (command.Connection.State == 0)
                {
                    command.Connection.Open();

                    if (parameters.Length > 0)
                        command.Parameters.AddRange(parameters);

                    await command.ExecuteNonQueryAsync();

                    command.Connection.Close();
                }

            }
        }

        public async Task<IList<SqlParameter>> ExecProcedureAsync(string StoredProcedure, SqlParameter[] parameters)
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

            await command.ExecuteNonQueryAsync();

            var rv = parameters.Where(x => x.Direction.Equals(ParameterDirection.Output) || x.Direction.Equals(ParameterDirection.ReturnValue)).ToList();
            rv.Add(returnParameter);

            sqlConnection.Close();

            return rv;
        }
    }

    public static class ExtensionMethods
    {
        public static decimal? ParseNullableDecimal(this string s)
        {
            if (decimal.TryParse(s, out decimal d))
                return d;
            return null;
        }

        public static string ToStringWithDBNull(this object o)
        {
            if (o is DBNull)
                return null;
            return o.ToString();
        }
    }

}
