using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ImisRestApi.Data;
using ImisRestApi.Escape.Payment.Models;
using ImisRestApi.Extensions;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;

namespace ImisRestApi.Formaters
{
    public class GePGXmlSerializerInputFormatter: TextInputFormatter
    {

        private Type type;
        static IConfiguration Configuration;

        public GePGXmlSerializerInputFormatter(IConfiguration configuration)
        {
            Configuration = configuration;
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
            string typeOfMessage = type.ToString().Split('.').Last();
            if (typeOfMessage != "GepgReconcMessage")
            {
                string billId = StringExtensions.Between(body, "<BillId>", "</BillId>");
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

        public static bool IsValidCall(object Reqbody, string responseType)
        {
            var _body = GetXmlStringFromObject(Reqbody);
            var body = _body.Replace(" />", "/>");
            var content = getContent(body, responseType);
            var signature = getSig(body, "gepgSignature");

            return VerifyData(content, signature);
        }

        private static bool VerifyData(string strContent, string strSignature)
        {
            try
            {
                var appRoot = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.LastIndexOf("\\bin"));
                var gepgPublicCertStorePath = Path.Combine(appRoot + Configuration["PaymentGateWay:GePG:GepgPublicCertStorePath"]);
                var gepgCertPass = Configuration["PaymentGateWay:GePG:GepgCertPass"];

                byte[] str = Encoding.UTF8.GetBytes(strContent);
                byte[] signature = Convert.FromBase64String(strSignature);

                X509Certificate2 certificate = new X509Certificate2(File.ReadAllBytes(gepgPublicCertStorePath), gepgCertPass);
                RSA rsaCrypto = (RSA)certificate.PublicKey.Key;

                SHA1Managed sha1hash = new SHA1Managed();
                byte[] hashdata = sha1hash.ComputeHash(str);

                if (rsaCrypto.VerifyHash(hashdata, signature, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static string GetXmlStringFromObject(object obj)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter tw = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                tw = new XmlTextWriter(sw);
                serializer.Serialize(tw, obj);
            }
            catch (Exception ex)
            {
                //Handle Exception Code
            }
            finally
            {
                sw.Close();
                if (tw != null)
                {
                    tw.Close();
                }
            }

            return sw.ToString();
        }

        private static string getContent(string rawData, string dataTag)
        {
            try
            {
                string content = rawData.Substring(rawData.IndexOf(dataTag) - 1, rawData.LastIndexOf(dataTag) + dataTag.Length + 2 - rawData.IndexOf(dataTag));
                return content;
            }
            catch (Exception)
            {

                return string.Empty;
            }

        }

        private static string getSig(string rawData, string sigTag)
        {
            try
            {
                string content = rawData.Substring(rawData.IndexOf(sigTag) + sigTag.Length + 1, rawData.LastIndexOf(sigTag) - rawData.IndexOf(sigTag) - sigTag.Length - 3);
                return content;
            }
            catch (Exception)
            {

                return string.Empty;
            }

        }
        #endregion
    }
}
