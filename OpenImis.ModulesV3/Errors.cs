﻿using System;
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
            Rejected,
            UnexpectedException = 2999
        }

        // Renewal Erros
        public enum Renewal
        {
            Accepted = 3001,
            AlreadyAccepted = 3002,
            Rejected = 3003,
            DuplicateReceiptFound = 3004,
            GracePeriodExpired = 3005,
            CouldNotUpdateControlNumber = 3006,
            InsuranceNumberNotFound = 3007,
            RenewalAlreadyRequested = 3008,
            NoPreviousPolicyFoundToRenew = 3009,
            UnexpectedException = 3999

        }
    }
}
