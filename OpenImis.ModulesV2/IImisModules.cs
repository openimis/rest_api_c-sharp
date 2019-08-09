﻿using OpenImis.ModulesV2.LoginModule;
using OpenImis.ModulesV2.ClaimModule;
using OpenImis.ModulesV2.InsureeModule;
using OpenImis.ModulesV2.CoverageModule;
using OpenImis.ModulesV2.PaymentModule;

namespace OpenImis.ModulesV2
{
    /// <summary>
    /// This interface serves to define a Service for the IMIS modules 
    /// </summary>
    public interface IImisModules
    {
        ILoginModule GetLoginModule();

        IClaimModule GetClaimModule();

        IInsureeModule GetInsureeModule();

        ICoverageModule GetCoverageModule();

        IPaymentModule GetPaymentModule();
    }
}