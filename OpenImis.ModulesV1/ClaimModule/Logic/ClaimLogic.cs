using OpenImis.DB.SqlServer;
using OpenImis.ModulesV1.ClaimModule.Models;
using OpenImis.ModulesV1.ClaimModule.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.ClaimModule.Logic
{
    public class ClaimLogic : IClaimLogic
    {
        protected IClaimRepository claimRepository;

        public ClaimLogic()
        {
            claimRepository = new ClaimRepository();
        }

        public DiagnosisServiceItem GetDsi(DsiInputModel model)
        {
            DiagnosisServiceItem message;

            message = claimRepository.GetDsi(model);

            return message;
        }

        public List<ClaimAdminModel> GetClaimAdministrators()
        {
            List<ClaimAdminModel> response = new List<ClaimAdminModel>();

            response = claimRepository.GetClaimAdministrators();

            return response;
        }

        public List<TblControls> GetControls()
        {
            List<TblControls> response = new List<TblControls>();

            response = claimRepository.GetControls();

            return response;
        }

        public PaymentLists GetPaymentLists(PaymentListsInputModel model)
        {
            PaymentLists response;

            response = claimRepository.GetPaymentLists(model);

            return response;
        }
    }
}
