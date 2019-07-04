using OpenImis.Modules.PaymentModule.Models;
using OpenImis.Modules.PaymentModule.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenImis.Modules.PaymentModule.Repositories
{
    public interface IPaymentRepository
    {
        string PaymentId { get; set; }
        decimal ExpectedAmount { get; set; }
        string ControlNum { get; set; }
        string PhoneNumber { get; set; }
        Language Language { get; set; }
        TypeOfPayment? typeOfPayment { get; set; }

        DateTime? PaymentDate { get; set; }
        decimal? PaidAmount { get; set; }
        decimal? OutStAmount { get; set; }
        List<InsureeProduct> InsureeProducts { get; set; }

        bool SaveControlNumberRequest(string BillId, bool failed);
        PostReqCNResponse PostReqControlNumber(string OfficerCode, string PaymentId, string PhoneNumber, decimal ExpectedAmount, List<PaymentDetail> products, string controlNumber = null, bool acknowledge = false, bool error = false);
        bool UpdatePaymentTransferFee(string paymentId, decimal TransferFee, TypeOfPayment typeOfPayment);
        decimal determineTransferFee(decimal expectedAmount, TypeOfPayment typeOfPayment);
        decimal determineTransferFeeReverse(decimal expectedAmount, TypeOfPayment typeOfPayment);
        int GetReqControlNumberAck(string paymentId);
        int GetPaymentDataAck(string paymentId, string controlNumber);
        decimal GetToBePaidAmount(decimal ExpectedAmount, decimal TransferFee);
        DataMessage SaveIntent(IntentOfPay _intent, int? errorNumber = 0, string errorMessage = null);
        DataMessage SaveControlNumber(string ControlNumber, bool failed);
        DataMessage SaveControlNumber(ControlNumberResp model, bool failed);
        bool CheckControlNumber(string PaymentID, string ControlNumber);
        void UpdateControlNumberStatus(string ControlNumber, CnStatus status);
        DataMessage SaveControlNumberAkn(bool error_occured, string Comment);
        DataMessage SavePayment(PaymentData payment, bool failed = false);
        DataMessage MatchPayment(MatchModel model);
        Task<DataMessage> GetControlNumbers(string PaymentIds);
        void GetPaymentInfo(string Id);
        List<MatchSms> GetPaymentIdsForSms();
        void UnMatchedSmsSent(int Id);
        void MatchedSmsSent();
        string GetPaymentId(string ControlNumber);
    }
}
