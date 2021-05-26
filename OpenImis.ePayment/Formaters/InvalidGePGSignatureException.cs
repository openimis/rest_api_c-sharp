using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ePayment.Formaters
{
    class InvalidGePGSignatureException:Exception
    {
        public InvalidGePGSignatureException()
        {
        }

        public InvalidGePGSignatureException(string message)
            : base(message)
        {
        }

        public InvalidGePGSignatureException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
