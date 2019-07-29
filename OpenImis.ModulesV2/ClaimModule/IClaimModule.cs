using OpenImis.ModulesV2.ClaimModule.Logic;

namespace OpenImis.ModulesV2.ClaimModule
{
    public interface IClaimModule
    {
        IClaimLogic GetClaimLogic();
        IClaimModule SetClaimLogic(IClaimLogic claimLogic);
    }
}
