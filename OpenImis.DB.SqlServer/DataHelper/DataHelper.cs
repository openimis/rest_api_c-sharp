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
        private readonly string _connectionString;
        private readonly int _commandTimeout = 30;

        public int ReturnValue { get; set; }

        public DataHelper(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:IMISDatabase"];
            if (configuration["ConnectionSettings:CommandTimeout"] != null) _commandTimeout = Int32.Parse(configuration["ConnectionSettings:CommandTimeout"]);
        }

        public DataSet FillDataSet(string SQL, SqlParameter[] parameters, CommandType commandType)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(SQL, sqlConnection) { CommandType = commandType, CommandTimeout = _commandTimeout })
            using (var adapter = new SqlDataAdapter(command))
            {
                DataSet ds = new DataSet();

                SqlParameter returnParameter = new SqlParameter("@RV", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                command.Parameters.Add(returnParameter);

                if (parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                adapter.Fill(ds);

                ReturnValue = int.Parse(returnParameter.Value.ToString());

                return ds;
            }
        }

        public DataTable GetDataTable(string SQL, SqlParameter[] parameters, CommandType commandType)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(SQL, sqlConnection) { CommandType = commandType, CommandTimeout = _commandTimeout })
            using (var adapter = new SqlDataAdapter(command))
            {
                DataTable dt = new DataTable();

                if (parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                adapter.Fill(dt);

                return dt;
            }
        }

        public DataSet GetDataSet(string SQL, SqlParameter[] parameters, CommandType commandType)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(SQL, sqlConnection) { CommandType = commandType, CommandTimeout = _commandTimeout })
            using (var adapter = new SqlDataAdapter(command))
            {
                DataSet ds = new DataSet();

                if (parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                adapter.Fill(ds);

                return ds;
            }
        }

        public void Execute(string SQL, SqlParameter[] parameters, CommandType commandType)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(SQL, sqlConnection) { CommandType = commandType, CommandTimeout = _commandTimeout })
            {
                sqlConnection.Open();

                if (parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                command.ExecuteNonQuery();
            }
        }

        public async Task ExecuteAsync(string SQL, SqlParameter[] parameters, CommandType commandType)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(SQL, sqlConnection) { CommandType = commandType, CommandTimeout = _commandTimeout })
            {
                if (command.Connection.State == 0)
                {
                    command.Connection.Open();

                    if (parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public ProcedureOutPut Procedure(string StoredProcedure, SqlParameter[] parameters, int tableIndex = 0)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(StoredProcedure, sqlConnection) { CommandType = CommandType.StoredProcedure, CommandTimeout = _commandTimeout })
            using (var adapter = new SqlDataAdapter(command))
            {
                DataSet dt = new DataSet();
                SqlParameter returnParameter = new SqlParameter("@RV", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                command.Parameters.Add(returnParameter);

                if (parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                adapter.Fill(dt);

                int rv = int.Parse(returnParameter.Value.ToString());

                DataTable dat = new DataTable();

                if (rv == 0 && dt.Tables.Count > tableIndex)
                {
                    dat = dt.Tables[tableIndex];
                }

                var output = new ProcedureOutPut { Code = rv, Data = dat };

                return output;
            }
        }

        public IList<SqlParameter> ExecProcedure(string StoredProcedure, SqlParameter[] parameters)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(StoredProcedure, sqlConnection) { CommandType = CommandType.StoredProcedure, CommandTimeout = _commandTimeout })
            {
                SqlParameter returnParameter = new SqlParameter("@RV", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                command.Parameters.Add(returnParameter);

                if (parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                sqlConnection.Open();

                command.ExecuteNonQuery();

                var rv = parameters.Where(x => x.Direction.Equals(ParameterDirection.Output) || x.Direction.Equals(ParameterDirection.ReturnValue)).ToList();
                rv.Add(returnParameter);

                return rv;
            }
        }

        public async Task<IList<SqlParameter>> ExecProcedureAsync(string StoredProcedure, SqlParameter[] parameters)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(StoredProcedure, sqlConnection) { CommandType = CommandType.StoredProcedure, CommandTimeout = _commandTimeout })
            {
                SqlParameter returnParameter = new SqlParameter("@RV", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnParameter);

                if (parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                sqlConnection.Open();

                await command.ExecuteNonQueryAsync();

                var rv = parameters.Where(x => x.Direction.Equals(ParameterDirection.Output) || x.Direction.Equals(ParameterDirection.ReturnValue)).ToList();
                rv.Add(returnParameter);

                return rv;
            }
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
