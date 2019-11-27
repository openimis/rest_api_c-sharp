using OpenImis.ModulesV1.ClaimModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.ClaimModule
{
    public interface IClaimModule
    {
        IClaimLogic GetClaimLogic();
        IClaimModule SetClaimLogic(IClaimLogic claimLogic);
    }
}
