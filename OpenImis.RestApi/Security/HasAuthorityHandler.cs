using Microsoft.AspNetCore.Authorization;
using OpenImis.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OpenImis.RestApi.Security
{
	public class HasAuthorityHandler : AuthorizationHandler<HasAuthorityRequirement>
	{
		IImisModules _imisModules;

		public HasAuthorityHandler(IImisModules imisModules)
		{
			_imisModules = imisModules;
		}

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasAuthorityRequirement requirement)
		{
			// If user does not have the scope claim, get out of here
			if (!context.User.HasClaim(c => c.Type == ClaimTypes.Name && c.Issuer == requirement.Issuer))
				return Task.CompletedTask;

			// Split the scopes string into an array
			//var scopes = context.User.FindFirst(c => c.Type == ClaimTypes.Name && c.Issuer == requirement.Issuer).Value.Split(' ');
			var username = context.User.FindFirst(claim => claim.Type == ClaimTypes.Name).Value;
			var scopes = _imisModules.GetUserModule().GetUserController().GetByUsername(username).GetRolesStringArray();

			// Succeed if the scope array contains the required scope
			if (scopes.Any(s => s == requirement.Authority))
				context.Succeed(requirement);

			return Task.CompletedTask;
		}
	}
}
