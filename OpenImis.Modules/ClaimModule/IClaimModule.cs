using OpenImis.Modules.ClaimModule.Logic;

namespace OpenImis.Modules.ClaimModule
{
    public interface IClaimModule
    {
        IClaimLogic GetClaimLogic();
        IClaimModule SetClaimLogic(IClaimLogic claimLogic);
    }
}
