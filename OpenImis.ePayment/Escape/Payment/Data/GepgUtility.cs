using OpenImis.ePayment.Escape.Payment.Models;
using OpenImis.ePayment.Models;
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
using Microsoft.Extensions.Logging;

namespace OpenImis.ePayment.Data
{

    public class GepgUtility
    {
        string PublicStorePath = string.Empty;
        string PrivateStorePath = string.Empty;
        string GepgPayCertStorePath = string.Empty;

        string CertPass = string.Empty;

        RSA rsaCrypto = null;
        gepgBillSubReq newBill = null;

        private IConfiguration configuration;
        private readonly ILogger _logger;

        public GepgUtility(IHostingEnvironment hostingEnvironment, IConfiguration Configuration, ILoggerFactory loggerFactory)
        {
            configuration = Configuration;
            _logger = loggerFactory.CreateLogger<GepgUtility>();

            PublicStorePath = Path.Combine(hostingEnvironment.ContentRootPath + Configuration["PaymentGateWay:GePG:PublicStorePath"]);
            PrivateStorePath = Path.Combine(hostingEnvironment.ContentRootPath + Configuration["PaymentGateWay:GePG:PrivateStorePath"]);
            GepgPayCertStorePath = Path.Combine(hostingEnvironment.ContentRootPath + Configuration["PaymentGateWay:GePG:GepgPayCertStorePath"]);

            CertPass = Configuration["PaymentGateWay:GePG:CertPass"];
        }

        public String CreateBill(IConfiguration Configuration, string OfficerCode, string PhoneNumber, int BillId, decimal ExpectedAmount, List<PaymentDetail> policies)
        {

            DataHelper dh = new DataHelper(Configuration);

            List<BillItem> items = new List<BillItem>();

            if (policies.Count > 0)
            {
                foreach (var policy in policies)
                {
                    BillItem item = new BillItem()
                    {
                        BillItemRef = policy.payment_detail_id.ToString(),
                        BillItemAmt = Convert.ToDouble(policy.amount),
                        BillItemEqvAmt = Convert.ToDouble(policy.amount),
                        BillItemMiscAmt = 0,
                        UseItemRefOnPay = "N",
                        GfsCode = Configuration["PaymentGateWay:GePG:GfsCode:0"]
                    };
                    items.Add(item);
                }
            }
            else
            {
                return "-2: error - no policy";
            }

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
                var InsureeNumber = policies.FirstOrDefault().insurance_number;

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
                    billTrxInf.PyrName = Convert.ToString(row["LastName"]) + " " + Convert.ToString(row["OtherNames"]);
                    billTrxInf.PyrEmail = "info@imis.co.tz";
                    billTrxInf.PyrCellNum = Convert.ToString(row["Phone"]).Length == 0 ? PhoneNumber : Convert.ToString(row["Phone"]);
                }
                else
                {
                    billTrxInf.PyrName = InsureeNumber;
                    billTrxInf.PyrEmail = "info@imis.co.tz"; // TODO: replace with insuree's email if exists
                    billTrxInf.PyrCellNum = PhoneNumber;
                }
            }
            else
            {
                var sSQL = @"SELECT Code,LastName,OtherNames,DOB,Phone,EmailId
                            FROM tblOfficer WHERE ValidityTo IS NULL AND Code = @OfficerCode";
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
                    billTrxInf.PyrEmail = "info@imis.co.tz"; // TODO: replace with officer's email 
                    billTrxInf.PyrCellNum = Convert.ToString(row["Phone"]).Length == 0 ? PhoneNumber : Convert.ToString(row["Phone"]);
                }

            }

            string accountCode = GetAccountCodeByProductCode(policies.FirstOrDefault().insurance_product_code);

            var billTrxInfs = new List<BillTrxInf> {
                billTrxInf
            };


            newBill = new gepgBillSubReq()
            {
                BillHdr = new BillHdr() { SpCode = accountCode, RtrRespFlg = true },
                BillTrxInf = billTrxInfs
            };

            var settings = new XmlWriterSettings() { OmitXmlDeclaration = true };
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var sb = new StringBuilder();
            var xs = new XmlSerializer(typeof(gepgBillSubReq));

            using (var xw = XmlWriter.Create(sb, settings))
            {
                try
                {
                    xs.Serialize(xw, newBill, ns);
                    xw.Flush();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception during XML serialization");
                }
                return sb.ToString();
            }
        }

        public String CreateGePGCancelPaymentRequest(IConfiguration Configuration, int PaymentId)
        {

            gepgBillCanclReq gepgBillCanclReq = new gepgBillCanclReq()
            {
                SpCode = GetAccountCodeByPaymentId(PaymentId),
                BillId = PaymentId.ToString(),
                SpSysId = Configuration["PaymentGateWay:GePG:SystemId"],
            };

            var xs = new XmlSerializer(typeof(gepgBillCanclReq));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var settings = new XmlWriterSettings() { OmitXmlDeclaration = true };
            var sb = new StringBuilder();

            using (var xw = XmlWriter.Create(sb, settings))
            {
                try
                {
                    xs.Serialize(xw, gepgBillCanclReq, ns);
                    xw.Flush();

                    var outString = sb.ToString();

                    var signature = this.GenerateSignature(outString);

                    GePGPaymentCancelRequest GePGPaymentCancelRequest = new GePGPaymentCancelRequest()
                    {
                        gepgBillCanclReq = gepgBillCanclReq,
                        gepgSignature = signature
                    };

                    xs = new XmlSerializer(typeof(GePGPaymentCancelRequest));
                    settings = new XmlWriterSettings();
                    sb = new StringBuilder();

                    using (var xw2 = XmlWriter.Create(sb, settings))
                    {
                        xs.Serialize(xw, gepgBillCanclReq, ns);
                        xw.Flush();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception during XML serialization");
                }

                return sb.ToString();
            }
        }

        public string FinaliseSignedMsg(string sign)
        {
            object gepgData = null;
            gepgData = new GepgBillMessage() { gepgBillSubReq = newBill, gepgSignature = sign };

            var xs = new XmlSerializer(typeof(GepgBillMessage));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var settings = new XmlWriterSettings();
            var sb = new StringBuilder();

            using (var xw = XmlWriter.Create(sb, settings))
            {
                try
                {
                    xs.Serialize(xw, gepgData, ns);
                    xw.Flush();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception during XML serialization");
                }
                return sb.ToString();
            }
        }

        public string FinaliseSignedMsg(object content, Type type)
        {
            //Gepg gepgBill = new Gepg() { Content = content, gepgSignature = sign };

            var xs = new XmlSerializer(type);
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var settings = new XmlWriterSettings();
            var sb = new StringBuilder();

            using (var xw = XmlWriter.Create(sb, settings))
            {
                try
                {
                    xs.Serialize(xw, content, ns);
                    xw.Flush();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception during XML serialization");
                }
                return sb.ToString();
            }
        }

        public string FinaliseSignedAcks(object content, Type type)
        {
            //Gepg gepgBill = new Gepg() { Content = content, gepgSignature = sign };

            var xs = new XmlSerializer(type);
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var settings = new XmlWriterSettings() { Encoding = Encoding.UTF8 };

            using (var sb = new MemoryStream())
            using (var xw = XmlWriter.Create(sb, settings))
            {
                try
                {
                    xs.Serialize(xw, content, ns);
                    xw.Flush();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception during XML serialization");
                }

                return Encoding.UTF8.GetString(sb.ToArray());
            }
        }

        public string SerializeClean(object bill, Type type)
        {
            var xs = new XmlSerializer(type);
            var sb = new StringBuilder();

            //To get rid of the default namespaces we create a new
            //set of namespaces with one empty entry.
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            //To get rid of the xml declaration we create an 
            //XmlWriterSettings object and tell it to OmitXmlDeclaration.
            var settings = new XmlWriterSettings() { OmitXmlDeclaration = true };

            //We create a new XmlWriter with the previously created settings 
            //(to OmitXmlDeclaration).
            using (var xw = XmlWriter.Create(sb, settings))
            {
                try
                {
                    //We call xs.Serialize and pass in our custom 
                    //XmlSerializerNamespaces object.
                    xs.Serialize(xw, bill, ns);
                    xw.Flush();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception during XML serialization");
                }

                return sb.ToString();
            }
        }

        public string GenerateSignature(string strUnsignedContent)
        {
            string signature = string.Empty;
            try
            {
                byte[] bts = File.ReadAllBytes(PrivateStorePath);
                X509Certificate2 certificate = new X509Certificate2(PrivateStorePath, CertPass);

                rsaCrypto = (RSA)certificate.PrivateKey;

                if (rsaCrypto != null)
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

        public async Task<string> SendHttpRequest(string FunctionURL, string Content, string SPCode, string GepgCom)
        {
            try
            {
                var url = configuration["PaymentGateWay:GePG:Url"];

                // Create POST data and convert it to a byte array.  
                byte[] byteArray = Encoding.UTF8.GetBytes(Content);
                
                WebRequest request = WebRequest.Create(url + FunctionURL);
                // Set the Method property of the request to POST.  
                request.Method = "POST";
                // Set the ContentType property of the WebRequest.  
                request.ContentType = "application/xml";
                // Set the ContentLength property of the WebRequest. 
                request.ContentLength = byteArray.Length;
                //Set Custom Headers
                request.Headers.Add("Gepg-Code", SPCode);
                request.Headers.Add("Gepg-Com", GepgCom);
                // Get the request stream.  
                using (Stream dataStream = request.GetRequestStream())
                {
                    // Write the data to the request stream.  
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }


                // Get the response.  
                WebResponse response = await request.GetResponseAsync();
                // Display the status.  
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.  
                using (var dataStream = response.GetResponseStream())
                using(var reader = new StreamReader(dataStream))
                {
                    string responseFromServer = reader.ReadToEnd();
                    Console.WriteLine(responseFromServer);

                    return responseFromServer;
                }   
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                _logger.LogError(ex, "Exception during web request");
                return ex.Message;
            }

        }

        public string GetAccountCodeByPaymentId(int PaymentId)
        {
            var getAccountCodeQuery = @"SELECT DISTINCT AccCodePremiums
                                          FROM tblProduct p
                                          inner join tblPaymentDetails pd on p.ProductCode=pd.ProductCode
                                          where p.ValidityTo is null and pd.PaymentID=@PaymentId";
            SqlParameter[] sqlParameters = {
                        new SqlParameter("@PaymentId", PaymentId),
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

        public string GetAccountCodeByProductCode(string ProductCode)
        {
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

        public async Task<string> CreateBulkBills(IConfiguration configuration, RequestBulkControlNumbersModel model)
        {
            var billTrxRefs = new List<BillTrxInf>();
            var rand = new Random();

            var sSQL = "uspPrepareBulkControlNumberRequests";
            SqlParameter[] parameters = {
                new SqlParameter("@Count", model.ControlNumberCount),
                new SqlParameter("@ProductCode", model.ProductCode),
                new SqlParameter("@ErrorCode", SqlDbType.Int){Direction = ParameterDirection.Output}
            };

            var dh = new DataHelper(configuration);

            DataTable dt = dh.GetDataTable(sSQL, parameters, CommandType.StoredProcedure);

            foreach (DataRow dr in dt.Rows)
            {
                var billItems = new List<BillItem>();
                BillItem item = new BillItem()
                {
                    BillItemRef = model.ProductCode,
                    BillItemAmt = Convert.ToDouble(dr["Amount"]),
                    BillItemEqvAmt = Convert.ToDouble(dr["Amount"]),
                    BillItemMiscAmt = 0,
                    UseItemRefOnPay = "N",
                    GfsCode = configuration["PaymentGateWay:GePG:GfsCode:0"]
                };
                billItems.Add(item);


                BillTrxInf billTrxInf = new BillTrxInf()
                {
                    BillId = (int)dr["BillId"],
                    SubSpCode = Convert.ToInt32(configuration["PaymentGateWay:GePG:SubSpCode"]),
                    SpSysId = configuration["PaymentGateWay:GePG:SystemId"],
                    Ccy = "TZS",
                    BillPayOpt = 3,
                    BillGenDt = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                    BillEqvAmt = Convert.ToDecimal(dr["Amount"]),
                    RemFlag = true,
                    BillExprDt = DateTime.Now.AddYears(3).ToString("yyyy-MM-ddTHH:mm:ss"),
                    BillAmt = Convert.ToDecimal(dr["Amount"]),
                    MiscAmt = 0,
                    BillItems = billItems,
                    BillDesc = "Bill",
                    BillApprBy = "Imis",
                    BillGenBy = "Imis"
                };

                billTrxInf.PyrId = dr["BillId"].ToString();
                billTrxInf.PyrName = "CHF IMIS";
                billTrxInf.PyrEmail = "info@imis.co.tz";
                billTrxInf.PyrCellNum = "";

                billTrxRefs.Add(billTrxInf);

            }

            string accountCode = GetAccountCodeByProductCode(model.ProductCode);

            newBill = new gepgBillSubReq();
            newBill.BillHdr = new BillHdr() { SpCode = accountCode, RtrRespFlg = true };
            newBill.BillTrxInf = billTrxRefs;

            var xs = new XmlSerializer(typeof(gepgBillSubReq));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var settings = new XmlWriterSettings() { OmitXmlDeclaration = true };

            using (var sb = new MemoryStream())
            using (var xw = XmlWriter.Create(sb, settings))
            {
                try
                {
                    xs.Serialize(xw, newBill, ns);
                    xw.Flush();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception during XML serialization");
                }

                return Encoding.UTF8.GetString(sb.ToArray());
            }
        }
    }
}
