using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ModulesV3
{
    public static class Errors
    {
        // Enrolment Errors

        // Claim Errors
        public enum Claim
        {
            Success = 2001,
            InvalidHFCode,
            DuplicateClaimCode,
            InvalidInsuranceNumber,
            EndDateIsBeforeStartDate,
            InvalidICDCode,
            InvalidItem,
            InvalidService,
            InvalidClaimAdmin,
            UnexpectedException = 2999
        }
    }
}
