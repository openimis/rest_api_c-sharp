using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.RestApi
{
    public class Errors
    {
        // Enrolment Errors

        // Claim Errors
        enum Claim
        {
            Success = 2001,
            InvalidHFCode,
            DuplicateClaimCode,
            InvalidInsuranceNumber,
            EndDateIsBeforeStartDate,
            InvalidICDCode,
            InvalidItem,
            InvalidService,
            InvalidClaimAdmin
        }
    }
}
