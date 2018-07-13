using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImisRestApi.Responses
{
    public class ValidateCredentialsResponse
    {
        public bool ErrorOccured { get; set; }
        public bool CredentialsValid { get; set; }
    }
}