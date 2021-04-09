using OpenImis.DB.SqlServer;
using OpenImis.Modules.ClaimModule.Models;
using OpenImis.Modules.ClaimModule.Models.RegisterClaim;
using System.Collections.Generic;

namespace OpenImis.Modules.ClaimModule.Logic
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
