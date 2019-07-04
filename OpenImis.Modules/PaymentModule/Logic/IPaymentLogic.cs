using OpenImis.Modules.PaymentModule.Models;
using OpenImis.Modules.PaymentModule.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenImis.Modules.PaymentModule.Logic
{
    public interface IPaymentLogic
    {
        Task<DataMessage> SaveIntent(IntentOfPay intent, int? errorNumber = 0, string errorMessage = null);
        Task<DataMessage> MatchPayment(MatchModel model);
        DataMessage SaveAcknowledgement(Acknowledgement model);
        Task<DataMessage> SavePayment(PaymentData model);
        DataMessage SaveControlNumber(ControlNumberResp model);
        void ControlNumberAssignedSms(IPaymentRepository payment);
        Task<DataMessage> GetControlNumbers(PaymentRequest requests);
        void ControlNumberNotassignedSms(IPaymentRepository payment, string error);
        void SendPaymentSms(PaymentRepository payment);
        void SendMatchSms(PaymentRepository payment);
        void SendMatchSms(List<MatchSms> Ids);
    }
}
