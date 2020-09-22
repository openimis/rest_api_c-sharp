using ImisRestApi.Chanels.Payment.Models;
using ImisRestApi.Data;
using ImisRestApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ImisRestApi.Data
{
    public class GepgUtility
    {
        string PrivateStorePath = string.Empty;
        string PublicStorePath = string.Empty;
        string GepgPublicCertStorePath = string.Empty;
        string GepgPayCertStorePath = string.Empty;

        string CertPass = "HPSS1234";
        string GepgCertPass = "gepg@2018";

        RSA rsaCrypto = null;
        gepgBillSubReq newBill = null;
        private IConfiguration configuration;

        public GepgUtility(IHostingEnvironment hostingEnvironment, IConfiguration Configuration)
        {
            configuration = Configuration;

            PrivateStorePath = hostingEnvironment.ContentRootPath + @"\Escape\Payment\Certificates\gepgclientprivatekey.pfx";
            PublicStorePath = hostingEnvironment.ContentRootPath + @"\Escape\Payment\Certificates\gepgclientpubliccertificate.pfx";
            GepgPublicCertStorePath = hostingEnvironment.ContentRootPath + @"\Escape\Payment\Certificates\gepgpubliccertificatetoclients.pfx";
            GepgPayCertStorePath = hostingEnvironment.ContentRootPath + @"\Escape\Payment\Certificates\gepgpaypubliccertificate.pfx";
        }

        public String CreateBill(IConfiguration Configuration, string OfficerCode,string PhoneNumber, string BillId, decimal ExpectedAmount, List<InsureeProduct> products)
        {

            DataHelper dh = new DataHelper(Configuration);

            List<BillItem> items = new List<BillItem>();

            BillItem item = new BillItem()
            {
                BillItemRef = "ImisPolicy",
                BillItemAmt = Convert.ToDouble(ExpectedAmount),
                BillItemEqvAmt = Convert.ToDouble(ExpectedAmount),
                BillItemMiscAmt = 0,
                UseItemRefOnPay = "N",
                GfsCode = Configuration["PaymentGateWay:GePG:GfsCode:0"]
            };

            items.Add(item);

            //foreach (var product in products)
            //{

            //    BillItem item = new BillItem()
            //    {
            //        BillItemRef = product.ProductCode,
            //        BillItemAmt = Convert.ToDouble(product.ExpectedProductAmount),
            //        BillItemEqvAmt = Convert.ToDouble(product.ExpectedProductAmount),
            //        BillItemMiscAmt = 0,
            //        UseItemRefOnPay = "N",
            //        GfsCode = Configuration["PaymentGateWay:GePG:GfsCode:0"]
            //    };

            //    items.Add(item);
            //}

            BillTrxInf billTrxInf = new BillTrxInf()
            {
                BillId = BillId,
                SubSpCode = Convert.ToInt32(Configuration["PaymentGateWay:GePG:SubSpCode"]),
                SpSysId = Configuration["PaymentGateWay:GePG:SystemId"],
                Ccy = "TZS",
                BillPayOpt = 3,
                BillGenDt = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                BillEqvAmt = Convert.ToDecimal(ExpectedAmount),
                RemFlag = true,
                BillExprDt = DateTime.Now.AddMonths(1).ToString("yyyy-MM-ddTHH:mm:ss"),
                BillAmt = Convert.ToDecimal(ExpectedAmount),
                MiscAmt = 0,
                BillItems = items,
                BillDesc = "Bill",
                BillApprBy = "Imis",
                BillGenBy = "Imis"
            };

            if (OfficerCode == null)
            {
                var InsureeNumber = products.FirstOrDefault().InsureeNumber;

                var sSQL = @"SELECT CHFID,LastName,OtherNames,Phone,Email
                             FROM tblInsuree WHERE CHFID = @InsureeNumber";

                SqlParameter[] parameters = {
                        new SqlParameter("@InsureeNumber", InsureeNumber),
                };

                var data = new DataHelper(Configuration);
                DataTable dt = data.GetDataTable(sSQL, parameters, CommandType.Text);

                billTrxInf.PyrId = InsureeNumber;
                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];                  
                    billTrxInf.PyrName = Convert.ToString(row["LastName"]) +" "+ Convert.ToString(row["OtherNames"]);
                    billTrxInf.PyrEmail = "info@imis.co.tz";
                    billTrxInf.PyrCellNum = Convert.ToString(row["Phone"]).Length == 0 ? PhoneNumber : Convert.ToString(row["Phone"]);
                }
                else
                {
                    billTrxInf.PyrName = InsureeNumber;
                    billTrxInf.PyrEmail = "info@imis.co.tz";
                    billTrxInf.PyrCellNum = PhoneNumber;
                }
            }
            else
            {
                var sSQL = @"SELECT Code,LastName,OtherNames,DOB,Phone,VEOCode,VEOLastName,VEOOtherNames,VEODOB,VEOPhone,EmailId
                            FROM tblOfficer WHERE Code = @OfficerCode";
                SqlParameter[] parameters = {
                        new SqlParameter("@OfficerCode", OfficerCode),
                };

                var data = new DataHelper(Configuration);
                DataTable dt = data.GetDataTable(sSQL, parameters, CommandType.Text);

                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];

                    billTrxInf.PyrId = OfficerCode;
                    billTrxInf.PyrName = Convert.ToString(row["LastName"]) + " " + Convert.ToString(row["OtherNames"]);
                    billTrxInf.PyrEmail = "info@imis.co.tz";
                    billTrxInf.PyrCellNum = Convert.ToString(row["VEOPhone"]).Length == 0 ? PhoneNumber : Convert.ToString(row["VEOPhone"]);
                }
  
            }

            string accountCode = getAccountCode(products);

            newBill = new gepgBillSubReq()
            {
                BillHdr = new BillHdr() { SpCode = accountCode, RtrRespFlg = true },
                BillTrxInf = billTrxInf
            };

            XmlSerializer xs = null;
            XmlSerializerNamespaces ns = null;
            XmlWriterSettings settings = null;
            String outString = String.Empty;

            XmlWriter xw = null;

            try
            {
                settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                //settings.Indent = true;
                ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                StringBuilder sb = new StringBuilder();
                xs = new XmlSerializer(typeof(gepgBillSubReq));

                xw = XmlWriter.Create(sb, settings);

                xs.Serialize(xw, newBill, ns);
                xw.Flush();
                outString = sb.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (xw != null)
                {
                    xw.Close();
                }
            }
            return outString;
        }

        public string FinaliseSignedMsg(string sign)
        {
            GepgBillMessage gepgBill = new GepgBillMessage() { gepgBillSubReq = newBill, gepgSignature = sign };

            XmlSerializer xs = null;
            XmlSerializerNamespaces ns = null;
            XmlWriterSettings settings = null;
            XmlWriter xw = null;
            String outString = String.Empty;

            try
            {
                ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                settings = new XmlWriterSettings();
                //settings.Indent = true;
                StringBuilder sb = new StringBuilder();
                xs = new XmlSerializer(typeof(GepgBillMessage));
                xw = XmlWriter.Create(sb, settings);

                xs.Serialize(xw, gepgBill, ns);
                xw.Flush();
                outString = sb.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (xw != null)
                {
                    xw.Close();
                }
            }

            return outString;
        }

        public string FinaliseSignedMsg(object content, Type type)
        {
            //Gepg gepgBill = new Gepg() { Content = content, gepgSignature = sign };

            XmlSerializer xs = null;
            XmlSerializerNamespaces ns = null;
            XmlWriterSettings settings = null;
            XmlWriter xw = null;
            String outString = String.Empty;

            try
            {


                ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                settings = new XmlWriterSettings();
                
                //settings.Indent = true;
                StringBuilder sb = new StringBuilder();
                xs = new XmlSerializer(type);
                xw = XmlWriter.Create(sb, settings);

                xs.Serialize(xw, content, ns);
                xw.Flush();
                outString = sb.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (xw != null)
                {
                    xw.Close();
                }
            }

            return outString;
        }

        public string FinaliseSignedAcks(object content, Type type)
        {
            //Gepg gepgBill = new Gepg() { Content = content, gepgSignature = sign };

            XmlSerializer xs = null;
            XmlSerializerNamespaces ns = null;
            XmlWriterSettings settings = null;
            XmlWriter xw = null;
            String outString = String.Empty;

            try
            {
                ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                settings = new XmlWriterSettings();
                settings.Encoding = System.Text.Encoding.UTF8;

                //settings.Indent = true;
                MemoryStream sb = new MemoryStream();
                xs = new XmlSerializer(type);
                xw = XmlWriter.Create(sb, settings);

                xs.Serialize(xw, content, ns);
                xw.Flush();
                outString = Encoding.UTF8.GetString(sb.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (xw != null)
                {
                    xw.Close();
                }
            }

            return outString;
        }

        public string SerializeClean(object bill,Type type)
        {
            XmlSerializer xs = null;
            //These are the objects that will free us from extraneous markup.
            XmlWriterSettings settings = null;
            XmlSerializerNamespaces ns = null;

            //We use a XmlWriter instead of a StringWriter.
            XmlWriter xw = null;

            String outString = String.Empty;

            try
            {
                //To get rid of the xml declaration we create an 
                //XmlWriterSettings object and tell it to OmitXmlDeclaration.
                settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;

                //To get rid of the default namespaces we create a new
                //set of namespaces with one empty entry.
                ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                StringBuilder sb = new StringBuilder();

                xs = new XmlSerializer(type);

                //We create a new XmlWriter with the previously created settings 
                //(to OmitXmlDeclaration).
                xw = XmlWriter.Create(sb, settings);

                //We call xs.Serialize and pass in our custom 
                //XmlSerializerNamespaces object.
                xs.Serialize(xw, bill, ns);

                xw.Flush();

                outString = sb.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (xw != null)
                {
                    xw.Close();
                }
            }
            return outString;
        }

        public string GenerateSignature(string strUnsignedContent)
        {
            string signature = string.Empty;
            try
            {
                byte[] bts = File.ReadAllBytes(PrivateStorePath);
                X509Certificate2 certificate = new X509Certificate2(PrivateStorePath, CertPass);

                rsaCrypto = (RSA)certificate.PrivateKey;

                if (rsaCrypto == null)
                {

                }
                else
                {
                    SHA1Managed sha1 = new SHA1Managed();
                    byte[] hash;

                    hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(strUnsignedContent));

                    byte[] signedHash = rsaCrypto.SignHash(hash, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
                    string signedHashString = Convert.ToBase64String(signedHash);
                    signature = signedHashString;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return signature;
        }

        public bool VerifyData(string strContent, string strSignature)
        {
            try
            {
                byte[] str = Encoding.UTF8.GetBytes(strContent);
                byte[] signature = Convert.FromBase64String(strSignature);

                X509Certificate2 certificate = new X509Certificate2(File.ReadAllBytes(GepgPublicCertStorePath), GepgCertPass);
                rsaCrypto = (RSA)certificate.PublicKey.Key;

                SHA1Managed sha1hash = new SHA1Managed();
                byte[] hashdata = sha1hash.ComputeHash(str);

               if (rsaCrypto.VerifyHash(hashdata,signature, HashAlgorithmName.SHA1,RSASignaturePadding.Pkcs1))
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

        public string SendHttpRequest(String content, List<InsureeProduct> products)
        {

            try
            {
                var url = configuration["PaymentGateWay:GePG:Url"];

                string accountCode = getAccountCode(products);
                // Create a request using a URL that can receive a post.   
                WebRequest request = WebRequest.Create(url+"/api/bill/sigqrequest");
                // Set the Method property of the request to POST.  
                request.Method = "POST";
                // Create POST data and convert it to a byte array.  
                byte[] byteArray = Encoding.UTF8.GetBytes(content);
                // Set the ContentType property of the WebRequest.  
                request.ContentType = "application/xml";
                // Set the ContentLength property of the WebRequest. 
                request.ContentLength = byteArray.Length;
                //Set Custom Headers
                request.Headers.Add("Gepg-Code", accountCode);
                request.Headers.Add("Gepg-Com", "default.sp.in");
                // Get the request stream.  
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.  
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.  
                dataStream.Close();


                // Get the response.  
                WebResponse response = request.GetResponse();
                // Display the status.  
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.  
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();
                // Display the content.  
                Console.WriteLine(responseFromServer);
                // Clean up the streams.  
                reader.Close();
                dataStream.Close();
                response.Close();
                return responseFromServer;
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message);
                return ex.Message;
            }

        }

        public string SendReconcHttpRequest(String content)
        {

            try
            {
                // Create a request using a URL that can receive a post.   
                WebRequest request = WebRequest.Create(configuration["PaymentGateWay:GePG:Url"]+"/api/reconciliations/sig_sp_qrequest");
                // Set the Method property of the request to POST.  
                request.Method = "POST";
                // Create POST data and convert it to a byte array.  
                byte[] byteArray = Encoding.UTF8.GetBytes(content);
                // Set the ContentType property of the WebRequest.  
                request.ContentType = "application/xml";
                // Set the ContentLength property of the WebRequest. 
                request.ContentLength = byteArray.Length;
                //Set Custom Headers
                request.Headers.Add("Gepg-Code", configuration["PaymentGateWay:GePG:SpCode"]);
                request.Headers.Add("Gepg-Com", "default.sp.in");
                // Get the request stream.  
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.  
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.  
                dataStream.Close();


                // Get the response.  
                WebResponse response = request.GetResponse();
                // Display the status.  
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.  
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();
                // Display the content.  
                Console.WriteLine(responseFromServer);
                // Clean up the streams.  
                reader.Close();
                dataStream.Close();
                response.Close();
                return responseFromServer;
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                return "";
            }

        }

        internal bool VerifyGePGData(string gepgResponse, int responseNo)
        {
            string dataTag = "gepgBillSubReqAck";

            string sigTag = "gepgSignature";

            string data = getContent(gepgResponse, dataTag);
            string signature = getSig(gepgResponse, sigTag);


            try
            {
                byte[] str = Encoding.UTF8.GetBytes(data);
                byte[] sig = Convert.FromBase64String(signature);

                // read the public key 
                X509Certificate2 certificate = new X509Certificate2(File.ReadAllBytes(GepgPublicCertStorePath), CertPass);
                rsaCrypto = (RSA)certificate.PublicKey.Key;

                // compute the hash again, also we can pass it as a parameter
                SHA1Managed sha1hash = new SHA1Managed();
                byte[] hashdata = sha1hash.ComputeHash(str);

                if (rsaCrypto.VerifyHash(hashdata, sig, HashAlgorithmName.SHA1, RSASignaturePadding.Pss))
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

        public string getContent(string rawData, string dataTag)
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

        public string getSig(string rawData, string sigTag)
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

        public bool VerifyPayData(string strContent, string strSignature)
        {
            try
            {
                byte[] str = Encoding.UTF8.GetBytes(strContent);
                byte[] signature = Convert.FromBase64String(strSignature);

                X509Certificate2 certificate = new X509Certificate2(File.ReadAllBytes(GepgPayCertStorePath), GepgCertPass);
                rsaCrypto = (RSA)certificate.PublicKey.Key;

                SHA1Managed sha1hash = new SHA1Managed();
                byte[] hashdata = sha1hash.ComputeHash(str);

                if (rsaCrypto.VerifyHash(hashdata, signature, HashAlgorithmName.SHA1, RSASignaturePadding.Pss))
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

        public string getAccountCode(List<InsureeProduct> products)
        {
            string ProductCode = products.FirstOrDefault().ProductCode;
            var getAccountCodeQuery = @"SELECT AccCodePremiums FROM tblProduct WHERE ProductCode = @ProductCode AND ValidityTo is NULL";
            SqlParameter[] sqlParameters = {
                        new SqlParameter("@ProductCode", ProductCode),
                };
            var sData = new DataHelper(configuration);
            string accountCode = "";
            DataTable results = sData.GetDataTable(getAccountCodeQuery, sqlParameters, CommandType.Text);
            if (results.Rows.Count > 0)
            {
                var result = results.Rows[0];
                if (!string.IsNullOrEmpty(Convert.ToString(result["AccCodePremiums"])))
                {
                    accountCode = Convert.ToString(result["AccCodePremiums"]);
                }
            }
            return accountCode;
        }

    }

    public class Gepg
    {
        public object Content { get; set; }
        public string gepgSignature { get; set; }
    }
}
