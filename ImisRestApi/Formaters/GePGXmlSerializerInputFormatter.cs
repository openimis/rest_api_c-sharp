using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ImisRestApi.Escape.Payment.Models;
using ImisRestApi.Extensions;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace ImisRestApi.Formaters
{
    public class GePGXmlSerializerInputFormatter: TextInputFormatter
    {

        private Type type;

        public GePGXmlSerializerInputFormatter()
        {

            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/xml"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        #region canreadtype
        protected override bool CanReadType(Type type)
        {
            this.type = type;
            if (type == typeof(GepgPaymentMessage) || type == typeof(GepgBillResponse) || type == typeof(GepgReconcMessage))
            {
                return base.CanReadType(type);
            }
            return false;
        }
        #endregion

        #region readrequest
        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding effectiveEncoding)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (effectiveEncoding == null)
            {
                throw new ArgumentNullException(nameof(effectiveEncoding));
            }

            var Request = context.HttpContext.Request;

            var buffer = new byte[Convert.ToInt32(Request.ContentLength)];
            string body = string.Empty;

            using (var reader = new StreamReader(
               Request.Body,
               encoding: Encoding.ASCII,
               detectEncodingFromByteOrderMarks: false,
               bufferSize: buffer.Length,
               leaveOpen: true
               ))
            {
                body = await reader.ReadToEndAsync();
                // stream = reader;// Do something
            }

            //get the billId/paymentId from request body - from <BillId> node
            string billId = StringExtensions.Between(body, "<BillId>", "</BillId>");
            string typeOfMessage = type.ToString().Split('.').Last();
            if (!String.IsNullOrEmpty(billId)) 
            {
                var gepgFile = new GepgFoldersCreating(billId + "_" + typeOfMessage, body, Path.Combine(System.Environment.CurrentDirectory, "wwwroot"));
                gepgFile.putRequestBody();
            }
            else
            {
                var gepgFile = new GepgFoldersCreating(typeOfMessage, body, Path.Combine(System.Environment.CurrentDirectory, "wwwroot"));
                gepgFile.putRequestBody();
            }

            TextReader writer = new StringReader(body);

            try
            {
                var serializer = new XmlSerializer(this.type);
                var model = Convert.ChangeType(serializer.Deserialize(writer), this.type);
                return await InputFormatterResult.SuccessAsync(model);
            }
            catch
            {
                return await InputFormatterResult.FailureAsync();
            }

        }
        #endregion
    }
}
