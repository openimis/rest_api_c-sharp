using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace OpenImis.RestApi.IntegrationTests.Helpers
{
    static class HttpBody
    {
        /// <summary>
        /// Creates a HTTP body from a JSON Object
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static ByteArrayContent GetBodyFromJSON(Object content)
        {
            var myContent = JsonConvert.SerializeObject(content);

            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return byteContent;
        }

		/// <summary>
		/// Creates a HTTP body from a JSON String
		/// </summary>
		/// <param name="content"></param>
		/// <returns></returns>
		public static ByteArrayContent GetBodyFromJSONString(string jsonString)
		{
			JObject jsonObject = new JObject();

			try
			{
				jsonObject = JObject.Parse(jsonString);
			}
			catch (Newtonsoft.Json.JsonReaderException e)
			{
				
			}

			return GetBodyFromJSON(jsonObject);
		}

		/// <summary>
		/// Creates a HTTP body from a JSON Object
		/// </summary>
		/// <param name="content"></param>
		/// <returns></returns>
		public static ByteArrayContent GetBodyFromString(string content)
		{
			var buffer = System.Text.Encoding.UTF8.GetBytes(content);
			var byteContent = new ByteArrayContent(buffer);

			byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			return byteContent;
		}
	}
}
