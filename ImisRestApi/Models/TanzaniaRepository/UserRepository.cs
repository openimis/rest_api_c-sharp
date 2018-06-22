using ImisRestApi.Models.Entities;
using ImisRestApi.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models.TanzaniaRepository
{
    /// <summary>
    /// This class is actual implementation of IUserRepository methods for Tanzania implementation 
    /// </summary>
    public class UserRepository: IUserRepository
    {

        public UserRepository()
        {
        }

        public TblUsers GetById(int userId)
        {
            return new TblUsers();
        }

        public TblUsers GetByUsername(string username)
        {
            TblUsers user;
            using (var imisContext = new IMISContext())
            {
                user = imisContext.TblUsers.Where(u => u.LoginName == username).FirstOrDefault();
            }
            return user;
        }

        public TblUsers GetByUsernameAndPassword(string username, string password)
        {
            TblUsers user;
            using (var imisContext = new IMISContext())
            {
                var userParameter = new SqlParameter("user", username);
                var passwordParameter = new SqlParameter("password", password);

                user = imisContext.TblUsers.FromSql($@"OPEN SYMMETRIC KEY EncryptionKey DECRYPTION BY Certificate EncryptData
                        SELECT *
                        FROM TblUsers
                        WHERE LoginName = @user
                        AND CONVERT(NVARCHAR(25), DECRYPTBYKEY(Password)) COLLATE LATIN1_GENERAL_CS_AS = @password
                        AND ValidityTo is null
                        CLOSE SYMMETRIC KEY EncryptionKey", userParameter, passwordParameter).SingleOrDefault();
            }
            return user;
        }
    }
}
