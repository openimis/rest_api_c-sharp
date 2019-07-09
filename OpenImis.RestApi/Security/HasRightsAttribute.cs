using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OpenImis.DB.SqlServer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OpenImis.RestApi.Security
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public class HasRightsAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        HashSet<Rights> userRights;

        public HasRightsAttribute(params Rights[] rights)
        {
            userRights = rights.ToHashSet();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            int userId = Convert.ToInt32(context.HttpContext.User.Claims
                .Where(w => w.Type == "UserId")
                .Select(x => x.Value)
                .FirstOrDefault());

            HashSet<int> rights;

            using (var imisContext = new ImisDB())
            {
                rights = (from UR in imisContext.TblUserRole
                          join RR in imisContext.TblRoleRight on UR.RoleID equals RR.RoleID
                          where UR.UserID == userId
                          select RR.RightID
                          ).ToHashSet();
            }

            bool isAuthorized = rights.All(x => userRights.Contains((Rights)Enum.ToObject(typeof(Rights), x)));

            if (!isAuthorized)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
