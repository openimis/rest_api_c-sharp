using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.RestApi.Security
{
	public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
	{
		private readonly AuthorizationOptions _options;
		private readonly IConfiguration _configuration;

		public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IConfiguration configuration) : base(options)
		{
			_options = options.Value;
			_configuration = configuration;
		}

		public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
		{
			// Check static policies first
			var policy = await base.GetPolicyAsync(policyName);

			if (policy == null)
			{
				policy = new AuthorizationPolicyBuilder()
					.AddRequirements(new HasAuthorityRequirement(policyName, _configuration["JwtIssuer"]))
					.Build();

				// Add policy to the AuthorizationOptions, so we don't have to re-create it each time
				_options.AddPolicy(policyName, policy);
			}

			return policy;
		}
	}
}
