using OpenImis.Modules.ClaimModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.ClaimModule
{
    public class ClaimModule : IClaimModule
    {
        private IClaimLogic _claimLogic;

        public ClaimModule()
        {
        }

        public IClaimLogic GetClaimLogic()
        {
            if (_claimLogic == null)
            {
                _claimLogic = new ClaimLogic();
            }
            return _claimLogic;
        }
    }
}
