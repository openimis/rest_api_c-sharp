using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Data
{
    public class DataHelper
    {
        private readonly string ConnectionString;


        public DataHelper(IConfiguration configuration)
        {
            ConnectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public DataTable GetDataTable(string SQL, SqlParameter[] parameters, CommandType commandType)
        {
            var dt = new DataTable();
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

        public int Procedure(string StoredProcedure, SqlParameter[] parameters)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand();
            SqlDataReader reader;

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
            // var message = new ResponseMessage(rv).Message;

            sqlConnection.Close();

            return rv;
        }

        public DataTable Login(string UserName, string Password)
        {
            var sSQL = @"OPEN SYMMETRIC KEY EncryptionKey DECRYPTION BY Certificate EncryptData
                        SELECT UserID,LoginName, LanguageID, RoleID, PrivateKey
                        FROM tblUsers
                        WHERE LoginName = @LoginName
                        AND  CONVERT(NVARCHAR(25), DECRYPTBYKEY(Password)) COLLATE LATIN1_GENERAL_CS_AS = @Password
                        AND ValidityTo is null
                        CLOSE SYMMETRIC KEY EncryptionKey";

            SqlParameter[] paramets = {
                new SqlParameter("@LoginName", UserName),
                new SqlParameter("@Password", Password)
            };

            //var data = new DataHelper(Configuration);

            var dt = GetDataTable(sSQL, paramets, CommandType.Text);

            return dt;
        }

    }
}
