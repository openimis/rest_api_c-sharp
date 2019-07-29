using OpenImis.DB.SqlServer;
using OpenImis.ModulesV1.ClaimModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.ClaimModule.Repositories
{
    public interface IClaimRepository
    {
        DiagnosisServiceItem GetDsi(DsiInputModel model);
        List<ClaimAdminModel> GetClaimAdministrators();
        List<TblControls> GetControls();
        PaymentLists GetPaymentLists(PaymentListsInputModel model);
    }
}
