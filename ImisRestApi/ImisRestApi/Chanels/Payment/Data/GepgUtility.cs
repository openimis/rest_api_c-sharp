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

namespace ImisRestApi.Chanels.Payment.Data
{
    public class GepgUtility
    {
        string PrivateStorePath = string.Empty;
        string PublicStorePath = string.Empty;
        string GepgPublicCertStorePath = string.Empty;
        string CertPass = "passpass";
        RSA rsaCrypto = null;
        gepgBillSubReq newBill = null;

        public GepgUtility(IHostingEnvironment hostingEnvironment)
        {
            PrivateStorePath = hostingEnvironment.ContentRootPath + @"\Certificates\gepgclientprivatekey.pfx";
            PublicStorePath = hostingEnvironment.ContentRootPath + @"\Certificates\gepgclientpubliccertificate.pfx";
            GepgPublicCertStorePath = hostingEnvironment.ContentRootPath + @"\Certificates\gepgpubliccertificate.pfx";
        }

        public String CreateBill(IConfiguration Configuration, string OfficerCode, string InsureeNumber, string BillId, double ExpectedAmount, List<PaymentDetail> products)
        {
            DataHelper dh = new DataHelper(Configuration);

            List<BillItem> items = new List<BillItem>();

            foreach (var product in products)
            {

                var policyStage = (product.PaymentType == 0) ? "r" : "n";

                SqlParameter[] _params = {
                    new SqlParameter("@ProductCode", product.ProductCode),
                    new SqlParameter("@InsureeNumber", product.InsureeNumber),
                    new SqlParameter("@PolicyStage", policyStage)
                };

                var query =@"DECLARE @ProductId INT
                             DECLARE @FamilyId INT
                             SET @ProductId =(SELECT ISNULL(ProdID,0) FROM tblProduct WHERE ProductCode = @ProductCode AND ValidityTo IS NULL)
                             SET @FamilyId =(SELECT ISNULL(FamilyID,0) FROM tblInsuree WHERE CHFID=@InsureeNumber AND ValidityTo IS NULL)
                             EXEC uspPolicyValue @familyid = @FamilyId, @prodid =@ProductId , @policystage= @PolicyStage";

                var _data = new DataHelper(Configuration);
                DataTable _dt = new DataTable();
                
                try
                {
                    _dt = _data.GetDataTable(query, _params, CommandType.Text);                 
                }
                catch (Exception e)
                {
                    throw new Exception();
                }

                var _row = _dt.Rows[0];

                BillItem item = new BillItem()
                {
                    BillItemRef = product.ProductCode,
                    BillItemAmt = Convert.ToDouble(_row["PolicyValue"]),
                    BillItemEqvAmt = Convert.ToDouble(_row["PolicyValue"]),
                    BillItemMiscAmt = 0,
                    UseItemRefOnPay = "N",
                    GfsCode = Configuration["PaymentGateWay:GePG:GfsCode:0"]
                };

                items.Add(item);
            }

            BillTrxInf billTrxInf = new BillTrxInf()
            {
                BillId = BillId,
                SubSpCode = Convert.ToInt32(Configuration["PaymentGateWay:GePG:SubSpCode"]),
                SpSysId = Configuration["PaymentGateWay:GePG:SystemId"],
                Ccy = "TZS",
                BillPayOpt = 1,
                BillGenDt = DateTime.UtcNow,
                BillEqvAmt = ExpectedAmount,
                RemFlag = true,
                BillExpDt = DateTime.UtcNow.AddMonths(1),
                BillAmt = ExpectedAmount,
                MiscAmt = 0,
                BillItems = items
            };

            if (OfficerCode == null)
            {
                var sSQL = @"SELECT CHFID,LastName,OtherNames,Phone,Email
                             FROM tblInsuree WHERE CHFID = @InsureeNumber";

                SqlParameter[] parameters = {
                        new SqlParameter("@InsureeNumber", InsureeNumber),
                };

                var data = new DataHelper(Configuration);
                DataTable dt = data.GetDataTable(sSQL, parameters, CommandType.Text);
                var row = dt.Rows[0];

                billTrxInf.PyrId = InsureeNumber;
                billTrxInf.PyrName = Convert.ToString(row["LastName"]) + Convert.ToString(row["OtherNames"]);
                
            }
            else
            {
                var sSQL = @"SELECT Code,LastName,OtherNames,DOB,Phone,VEOCode,VEOLastName,VEOOtherNames,VEODOB,VEOPhone
                            FROM tblOfficer WHERE Code = @OfficerCode";
                SqlParameter[] parameters = {
                        new SqlParameter("@OfficerCode", OfficerCode),
                };

                var data = new DataHelper(Configuration);
                DataTable dt = data.GetDataTable(sSQL, parameters, CommandType.Text);
                var row = dt.Rows[0];

                billTrxInf.PyrId = OfficerCode;
                billTrxInf.PyrName = Convert.ToString(row["LastName"]) + Convert.ToString(row["OtherNames"]);
               
            }

            newBill = new gepgBillSubReq()
            {
                BillHdr = new BillHdr() { SpCode = "SP257", RtrRespFlg = true },
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

                X509Certificate2 certificate = new X509Certificate2(File.ReadAllBytes(PrivateStorePath), CertPass);
                //certificate.Import(PrivateStorePath, CertPass, X509KeyStorageFlags.PersistKeySet);

                rsaCrypto = (RSA)certificate.PrivateKey;

                if (rsaCrypto == null)
                {

                }
                else
                {
                    SHA1Managed sha1 = new SHA1Managed();
                    byte[] hash;

                    hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(strUnsignedContent));

                    // byte[] signedHash = rsaCrypto.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
                    byte[] signedHash = rsaCrypto.SignHash(hash, HashAlgorithmName.SHA1, RSASignaturePadding.Pss);
                    string signedHashString = Convert.ToBase64String(signedHash);
                    signature = signedHashString;
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

            return signature;
        }

        public bool VerifyData(string strContent, string strSignature)
        {
            try
            {
                byte[] str = Encoding.UTF8.GetBytes(strContent);
                byte[] signature = Convert.FromBase64String(strSignature);

                X509Certificate2 certificate = new X509Certificate2();
                certificate.Import(PublicStorePath, CertPass, X509KeyStorageFlags.PersistKeySet);
                rsaCrypto = (RSACryptoServiceProvider)certificate.PublicKey.Key;

                SHA1Managed sha1hash = new SHA1Managed();
                byte[] hashdata = sha1hash.ComputeHash(str);

               // if (rsaCrypto.VerifyHash(hashdata, "SHA1", signature))
               if (rsaCrypto.VerifyHash(hashdata,signature, HashAlgorithmName.SHA1,RSASignaturePadding.Pss))
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

        public string SendHttpRequest(String content)
        {

            try
            {
                // Create a request using a URL that can receive a post.   
                WebRequest request = WebRequest.Create("http://154.118.230.18:80/api/bill/sigqrequest");
                // Set the Method property of the request to POST.  
                request.Method = "POST";
                // Create POST data and convert it to a byte array.  
                byte[] byteArray = Encoding.UTF8.GetBytes(content);
                // Set the ContentType property of the WebRequest.  
                request.ContentType = "application/xml";
                // Set the ContentLength property of the WebRequest. 
                request.ContentLength = byteArray.Length;
                //Set Custom Headers
                request.Headers.Add("Gepg-Code", "SP108");
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

        internal bool VerifyGePGData(string gepgResponse)
        {
            string dataTag = "gepgBillSubReqAck";
            string sigTag = "gepgSignature";

            string data = getContent(gepgResponse, dataTag);
            string signature = getSig(gepgResponse, sigTag);

           // MessageBox.Show(data, "Received Content");
           // MessageBox.Show(signature, "Received Signature");

            try
            {
                byte[] str = Encoding.UTF8.GetBytes(data);
                byte[] sig = Convert.FromBase64String(signature);

                // read the public key 
                X509Certificate2 certificate = new X509Certificate2();
                certificate.Import(GepgPublicCertStorePath, CertPass, X509KeyStorageFlags.PersistKeySet);
                rsaCrypto = (RSACryptoServiceProvider)certificate.PublicKey.Key;

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
               // MessageBox.Show(ex.Message);
                return false;
            }

        }

        private string getContent(string rawData, string dataTag)
        {
            string content = rawData.Substring(rawData.IndexOf(dataTag) - 1, rawData.LastIndexOf(dataTag) + dataTag.Length + 2 - rawData.IndexOf(dataTag));
            return content;
        }

        private string getSig(string rawData, string sigTag)
        {
            string content = rawData.Substring(rawData.IndexOf(sigTag) + sigTag.Length + 1, rawData.LastIndexOf(sigTag) - rawData.IndexOf(sigTag) - sigTag.Length - 3);
            return content;
        }

 
    }

    public class Gepg
    {
        public object Content { get; set; }
        public string gepgSignature { get; set; }
    }
}
