
namespace OpenImis.ePayment.Formaters
{
    public static class GepgCodeResponses
    {
        public static int Successful = 7101;
        public static int Failure = 7201;
        public static int HeaderNotGiven = 7202;
        public static int Unauthorized = 7203;
        public static int BillNotExist = 7204;
        public static int InvalidServiceProvider = 7205;
        public static int ServiceProviderNotActive = 7206;
        public static int DuplicatePayment = 7207;
        public static int InvalidBusinessAccount = 7208;
        public static int BusinessAccountNotActive = 7209;
        public static int CollectionAccountBalanceLimitReached = 7210;
        public static int PaymentServiceProviderCodeNotMatchBillServiceProviderCode = 7211;
        public static int PaymentCurrencyDidNotMatchBillCurrency = 7212;
        public static int BillHasExpired = 7213;
        public static int InsufficientAmountPaid = 7214;
        public static int InvalidRequestData = 7242;
        public static int InvalidSignature = 7303;
    }
}
