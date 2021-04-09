using OpenImis.Modules.PaymentModule.Models;
using OpenImis.Modules.PolicyModule.Models;
using System.Threading.Tasks;

namespace OpenImis.Modules.PaymentModule.Logic
{
    public interface IPaymentLogic
    {
        string WebRootPath { get; set; }
        string ContentRootPath { get; set; }

        Task<DataMessage> GetControlNumbers(PaymentRequest requests);
        Task<DataMessage> SaveIntent(IntentOfPay intent, int? errorNumber = 0, string errorMessage = null);
    }
}
