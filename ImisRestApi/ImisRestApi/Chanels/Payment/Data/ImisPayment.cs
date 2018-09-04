using ImisRestApi.Chanels.Payment.Data;
using ImisRestApi.Chanels.Payment.Models;
using ImisRestApi.Data;
using ImisRestApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ImisRestApi.Data
{
    public class ImisPayment
    {
        private IConfiguration Configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ImisPayment(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }


        public string GenerateCtrlNoRequest(string OfficerCode, string InsureeNumber,string BillId, double ExpectedAmount, List<PaymentDetail> products)
        {

            GepgUtility gepg = new GepgUtility(_hostingEnvironment);
            var bill = gepg.CreateBill(Configuration, OfficerCode, InsureeNumber, BillId, ExpectedAmount, products);
            var signature = gepg.GenerateSignature(bill);
            var signedMesg = gepg.FinaliseSignedMsg(bill, signature);
            var request = gepg.SendHttpRequest(signedMesg);
            return "Ok";
          
        }

        public string GenerateSignature(string unsignedContent) {
            string signature = string.Empty;

            try
            {
                X509Certificate2 certificate = new X509Certificate2();
                certificate.Import("", "", X509KeyStorageFlags.PersistKeySet); // contants needed

                var Crypto = (RSACryptoServiceProvider)certificate.PrivateKey;

                if(Crypto == null)
                {
                    throw new Exception();
                }
                else
                {
                    SHA1Managed sha1 = new SHA1Managed();
                    byte[] hash;

                    hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(unsignedContent)); // contants needed

                    byte[] signedHash = Crypto.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
                    string signedHashString = Convert.ToBase64String(signedHash);

                    signature = signedHashString;
                }

            }
            catch (Exception)
            {

                throw;
            }

            return signature;
        }
    }
}
