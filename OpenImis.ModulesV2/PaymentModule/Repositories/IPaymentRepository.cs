using OpenImis.ModulesV2.PaymentModule.Models;
using OpenImis.ModulesV2.PaymentModule.Models.Response;
using OpenImis.ModulesV2.PolicyModule.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenImis.ModulesV2.PaymentModule.Repositories
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

        DataMessage SaveIntent(IntentOfPay _intent, int? errorNumber = 0, string errorMessage = null);
        Task<DataMessage> GetControlNumbers(string PaymentIds);
        void GetPaymentInfo(string Id);
        decimal determineTransferFee(decimal expectedAmount, TypeOfPayment typeOfPayment);
        bool UpdatePaymentTransferFee(string paymentId, decimal TransferFee, TypeOfPayment typeOfPayment);
        decimal GetToBePaidAmount(decimal ExpectedAmount, decimal TransferFee);
        PostReqCNResponse PostReqControlNumber(string OfficerCode, string PaymentId, string PhoneNumber, decimal ExpectedAmount, List<PaymentDetail> products, string controlNumber = null, bool acknowledge = false, bool error = false);
        bool CheckControlNumber(string PaymentID, string ControlNumber);
        DataMessage SaveControlNumberAkn(bool error_occured, string Comment);
        DataMessage SaveControlNumber(string ControlNumber, bool failed);
        DataMessage SaveControlNumber(ControlNumberResp model, bool failed);

    }
}
