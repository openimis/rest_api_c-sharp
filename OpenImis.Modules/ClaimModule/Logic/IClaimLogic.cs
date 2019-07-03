using OpenImis.DB.SqlServer;
using OpenImis.Modules.ClaimModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.ClaimModule.Logic
{
    public interface IClaimLogic
    {
        DiagnosisServiceItem GetDsi(DsiInputModel model);
        List<ClaimAdminModel> GetClaimAdministrators();
        List<TblControls> GetControls();
        PaymentLists GetPaymentLists(PaymentListsInputModel model);
    }
}
