using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace OpenImis.ePayment.Formaters
{
    class GePGSignatureValidator
    {

        string GepgPublicCertStorePath = string.Empty;
        string GepgCertPass = string.Empty;

        public GePGSignatureValidator(IHostingEnvironment hostingEnvironment, IConfiguration Configuration)
        {
            GepgPublicCertStorePath = Path.Combine(hostingEnvironment.ContentRootPath + Configuration["PaymentGateWay:GePG:GepgPublicCertStorePath"]);
            
            GepgCertPass = Configuration["PaymentGateWay:GePG:GepgCertPass"];
        }

        public bool VerifyData(string strContent, string strSignature)
        {
            RSA rsaCrypto = null;
            try
            {
                byte[] str = Encoding.UTF8.GetBytes(strContent);
                byte[] signature = Convert.FromBase64String(strSignature);

                X509Certificate2 certificate = new X509Certificate2(File.ReadAllBytes(GepgPublicCertStorePath), GepgCertPass);
                rsaCrypto = (RSA)certificate.PublicKey.Key;

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
    }
}
