
namespace OpenImis.ePayment.Escape.Payment.Models
{
    public static class PaymentStatus
    {
        public static int FailedReconciliated  = -5;
        public static int FailedReceived = -4;
        public static int FailedControlNumberReceived = -3;
        public static int FailedSent = -2;
        public static int FailedRequested = -1;
        public static int Cancelled = 0;
        public static int Requested = 1;
        public static int Sent = 2;
        public static int ControlNumberReceived = 3;
        public static int Received = 4;
        public static int Reconciliated = 5;
    }
}
