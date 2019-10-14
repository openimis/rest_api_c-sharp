using OpenImis.DB.SqlServer;
using OpenImis.ModulesV2.ClaimModule.Models;
using OpenImis.ModulesV2.ClaimModule.Models.RegisterClaim;
using System.Collections.Generic;

namespace OpenImis.ModulesV2.ClaimModule.Logic
{
    public interface IClaimLogic
    {
        int Create(Claim claim);
        DiagnosisServiceItem GetDsi(DsiInputModel model);
        List<ClaimAdminModel> GetClaimAdministrators();
        List<TblControls> GetControls();
        PaymentLists GetPaymentLists(PaymentListsInputModel model);
    }
}
