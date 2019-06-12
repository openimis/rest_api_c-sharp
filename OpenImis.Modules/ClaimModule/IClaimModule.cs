using OpenImis.Modules.ClaimModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.ClaimModule
{
    public interface IClaimModule
    {
        IClaimLogic GetClaimLogic();
    }
}
