using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV2.PaymentModule.Models;

namespace OpenImis.ModulesV2.PaymentModule.Helpers.SMS
{
    public class ImisSms : ImisBaseSms
    {
        public ImisSms(IConfiguration config, string webRootPath, string contentRootPath, Language language = Language.Primary) : base(config, webRootPath, contentRootPath, language)
        {

        }
    }
}
