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
            Guid userId = Guid.Parse(context.HttpContext.User.Claims
                .Where(w => w.Type == "UserUUID")
                .Select(x => x.Value)
                .FirstOrDefault());

            HashSet<int> rights;

            using (var imisContext = new ImisDB())
            {
                rights = (from UR in imisContext.TblUserRole
                          join RR in imisContext.TblRoleRight.Where(x => x.ValidityTo == null) on UR.RoleID equals RR.RoleID
                          join US in imisContext.TblUsers.Where(x => x.ValidityTo == null) on UR.UserID equals US.UserId
                          where (US.UserUUID == userId && UR.ValidityTo == null)
                          select RR.RightID
                          ).ToHashSet();
            }

            bool isAuthorized = userRights.Select(s => (int)s).All(x => rights.Contains(x));

            if (!isAuthorized)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
