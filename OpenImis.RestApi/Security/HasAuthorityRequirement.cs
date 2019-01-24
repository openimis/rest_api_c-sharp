using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.RestApi.Security
{
	public class HasAuthorityRequirement : IAuthorizationRequirement
	{
		public string Issuer { get; }
		public string Authority { get; }

		public HasAuthorityRequirement(string authority, string issuer)
		{
			Authority = authority ?? throw new ArgumentNullException(nameof(authority));
			Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
		}
	}
}
