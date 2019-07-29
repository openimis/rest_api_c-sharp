using OpenImis.ModulesV2.ClaimModule.Logic;

namespace OpenImis.ModulesV2.ClaimModule
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

        public IClaimModule SetClaimLogic(IClaimLogic claimLogic)
        {
            _claimLogic = claimLogic;
            return this;
        }
    }
}
