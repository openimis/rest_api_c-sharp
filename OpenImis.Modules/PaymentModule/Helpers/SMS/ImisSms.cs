using Microsoft.Extensions.Configuration;
using OpenImis.Modules.PaymentModule.Models;

namespace OpenImis.Modules.PaymentModule.Helpers.SMS
{
    public class ImisSms : ImisBaseSms
    {
        public ImisSms(IConfiguration config, string webRootPath, string contentRootPath, Language language = Language.Primary) : base(config, webRootPath, contentRootPath, language)
        {

        }
    }
}
