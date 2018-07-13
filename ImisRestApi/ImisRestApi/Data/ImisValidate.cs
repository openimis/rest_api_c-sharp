using ImisRestApi.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ImisRestApi.Data
{
    public class ImisValidate
    {
        private IConfiguration Configuration;

        public ImisValidate(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public DataTable Credentials(UserLogin model)
        {
            DataHelper helper = new DataHelper(Configuration);

            var sSQL = @"OPEN SYMMETRIC KEY EncryptionKey DECRYPTION BY Certificate EncryptData
                        SELECT UserID, LoginName, LanguageID, RoleID
                        FROM tblUsers
                        WHERE LoginName = @LoginName
                        AND CONVERT(NVARCHAR(25), DECRYPTBYKEY(Password)) COLLATE LATIN1_GENERAL_CS_AS = @Password
                        AND ValidityTo is null
                        CLOSE SYMMETRIC KEY EncryptionKey";

            SqlParameter[] sqlParameters = {
                new SqlParameter("@LoginName", model.UserID),
                new SqlParameter("@Password", model.Password)
            };

            var response = helper.GetDataTable(sSQL, sqlParameters,CommandType.Text);

            return response;

        }
    }
}