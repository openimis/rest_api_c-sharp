using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using OpenImis.RestApi.IntegrationTests.Helpers;
using OpenImis.RestApi.Protocol.LoginModel;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace OpenImis.RestApi.IntegrationTests
{
    public class LoginTests
    {
        private TestFixture _testFixture;
		 const string LOGIN_ROUTE = "/api/login";


		public LoginTests()
        {
            _testFixture = new TestFixture();
        }
		
		/// <summary>
		/// Given  that the following users exists
		/// | Username | Password |
		/// | Admin    | Admin    |
		/// When I POST /api/login with username Admin and password Admin
		/// Then the return code is OK (200) 
		/// And the body contains a JSON object with a token and an exiration date
		/// </summary>
		/// <returns></returns>
		[Theory]
		[InlineData(@"{'username': 'Admin', 'password': 'Admin'}")]
		public async Task LoginWithGoodCredentials(string credentials) 
        {
			// SETUP
			ByteArrayContent content = HttpBody.GetBodyFromJSONString(credentials);

			// ACT
			var response = await _testFixture.Client.PostAsync(LOGIN_ROUTE, content);
            
			// ASSERT
			response.StatusCode.Should().Be(HttpStatusCode.OK);

			var jsonString = await response.Content.ReadAsStringAsync();
			var jsonObject = JObject.Parse(jsonString);

			jsonObject.Should().HaveElement("token").And.HaveElement("expires");

			LoginResponseModel loginResponse = jsonObject.ToObject<LoginResponseModel>();

			loginResponse.Token.Should().NotBeEmpty();
			loginResponse.Expires.Should().BeAfter(DateTime.Now);
		}

		[Theory]
		[InlineData(@"{'username': 'Admin', 'password': 'Admin123'}")]
		[InlineData(@"{'username': 'Admin123', 'password': 'Admin'}")]
		[InlineData(@"{'username': 'Admin123', 'password': 'Admin123'}")]
		public async Task LoginWithBadCredentials(string credentials)
		{
			// SETUP
			ByteArrayContent content = HttpBody.GetBodyFromJSONString(credentials);

			// ACT
			var response = await _testFixture.Client.PostAsync(LOGIN_ROUTE, content);

			// ASSERT
			response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
		}

		[Theory]
		[InlineData("{'username': 'Admin'}")]
		[InlineData("{'user': 'Admin', 'pass': 'Admin'}")]
		[InlineData("{'password': 'Admin'}")]
		[InlineData("{}")]
		public async Task LoginWithIncompleteCredentials(string credentials)
		{
			// SETUP
			JObject credentialObject = JObject.Parse(credentials);
			ByteArrayContent content = HttpBody.GetBodyFromJSON(credentialObject);

			// ACT
			var response = await _testFixture.Client.PostAsync(LOGIN_ROUTE, content);

			// ASSERT
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

			var jsonString = await response.Content.ReadAsStringAsync();
			var jsonObject = JObject.Parse(jsonString);

			if (!credentialObject.ContainsKey("username"))
			{
				jsonObject.Should().HaveElement("Username");
			}

			if (!credentialObject.ContainsKey("password"))
			{
				jsonObject.Should().HaveElement("Password");
			}
			
		}

		[Theory]
		[InlineData("")]
		[InlineData("{'username': 'Admin', 'password': 'Admin'")]
		public async Task LoginWithNonJsonBody(string credentials)
		{
			// SETUP
			var credentialObject = new JObject();
			ByteArrayContent content;

			try
			{
				credentialObject = JObject.Parse(credentials);
				content = HttpBody.GetBodyFromJSON(credentialObject);
			}
			catch (Newtonsoft.Json.JsonReaderException e)
			{
				content = HttpBody.GetBodyFromString("");
			}

			// ACT
			var response = await _testFixture.Client.PostAsync(LOGIN_ROUTE, content);

			// ASSERT
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

			var jsonString = await response.Content.ReadAsStringAsync();
			var jsonObject = JObject.Parse(jsonString);

			jsonObject.Should().HaveElement("");

		}
	}
}
