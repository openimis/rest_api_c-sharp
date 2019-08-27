using OpenImis.ModulesV2.ClaimModule.Models.RegisterClaim;

namespace OpenImis.ModulesV2.ClaimModule.Logic
{
    public interface IClaimLogic
    {
        int Create(Claim claim);
    }
}
