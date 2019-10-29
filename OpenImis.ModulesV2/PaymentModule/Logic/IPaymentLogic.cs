using OpenImis.ModulesV2.PaymentModule.Models;
using OpenImis.ModulesV2.PolicyModule.Models;
using System.Threading.Tasks;

namespace OpenImis.ModulesV2.PaymentModule.Logic
{
    public interface IPaymentLogic
    {
        string WebRootPath { get; set; }
        string ContentRootPath { get; set; }

        Task<DataMessage> GetControlNumbers(PaymentRequest requests);
        Task<DataMessage> SaveIntent(IntentOfPay intent, int? errorNumber = 0, string errorMessage = null);
    }
}
