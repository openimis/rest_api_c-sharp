using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OpenImis.RestApi.Util.ErrorHandling
{
    /// <summary>
    /// Exception for business related errors. It's error message will be included in the payload.
    /// </summary>
    public class BusinessException : Exception
    {
        public HttpStatusCode Status { get; }

        /// <summary>
        /// Default business exception. Returns 400BadRequest.
        /// </summary>
        public BusinessException(string message) : this(HttpStatusCode.BadRequest, message)
        {
        }

        public BusinessException(HttpStatusCode status, string message) : base(message)
        {
            Status = status;
        }
    }
}
