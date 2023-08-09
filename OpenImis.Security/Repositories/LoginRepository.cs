using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.Security.Models;
using OpenImis.Security.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenImis.Security.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private IConfiguration Configuration;

        public LoginRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public UserData GetByUUID(Guid userUUID)
        {
            UserData user;
            using (var imisContext = new ImisDB())
            {
                user = imisContext.TblUsers.Where(u => u.UserUUID == userUUID).Select(x => new UserData()
                {
                    UserUUID = x.UserUUID,
                    LoginName = x.LoginName,
                    PrivateKey = x.PrivateKey,
                    StoredPassword = x.StoredPassword
                })
                .FirstOrDefault();
            }

            return user;
        }

        public List<UserData> FindUserByName(string UserName)
        {
            List<UserData> response = new List<UserData>();

            using (var imisContext = new ImisDB())
            {
                response = imisContext.TblUsers
                                .Where(u => u.LoginName == UserName && u.ValidityTo == null)
                                .Select(x => new UserData()
                                {
                                    UserUUID = x.UserUUID,
                                    LoginName = Convert.ToString(x.LoginName),
                                    PrivateKey = Convert.ToString(x.PrivateKey),
                                    StoredPassword = Convert.ToString(x.StoredPassword)
                                })
                                .ToList();
            }

            return response;
        }

        public UserModel GetUserDetails(Guid userUUID)
        {
            var user = new UserModel();
            using(var imisContext = new ImisDB())
            {
               user = imisContext.TblUsers
                    .Include(ud => ud.TblUsersDistricts).ThenInclude(l => l.Location).Where(l => l.ValidityTo == null)
                    .Where(usr => usr.UserUUID == userUUID)
                    .Select(x => new UserModel()
                    {
                        LoginName = x.LoginName,
                        OtherNames = x.OtherNames,
                        LastName = x.LastName,
                        EmailId = x.EmailId,
                        Locations = x.TblUsersDistricts.Select(l =>  new Location() { 
                            LocationId = l.Location.LocationId,
                            Code = l.Location.LocationCode,
                            Name = l.Location.LocationName,
                            Type = l.Location.LocationType
                        }).ToList()
                    })
                    .FirstOrDefault();

                var rights =
                    (from u in imisContext.TblUsers
                    join userRole in imisContext.TblUserRole on u.UserId equals userRole.UserID
                    join role in imisContext.TblRole on userRole.RoleID equals role.RoleId
                    join right in imisContext.TblRoleRight on role.RoleId equals right.RoleID
                    where u.UserUUID == userUUID && u.ValidityTo == null && userRole.ValidityTo == null && role.ValidityTo == null && right.ValidityTo == null
                    select right.RightID
                    ).Distinct();

                
                user.Rights = rights.ToList();

            }

            return user;
        }
    }
}
