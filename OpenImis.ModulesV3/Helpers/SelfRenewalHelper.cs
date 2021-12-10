using Microsoft.EntityFrameworkCore;
using OpenImis.DB.SqlServer;
using OpenImis.ModulesV3.PolicyModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenImis.ModulesV3.Helpers
{

    class SelfRenewalHelper
    {

        public DataMessage CreateSelfRenewal(SelfRenewal renewal)
        {
            var dataMessage = Validate(renewal);

            if (dataMessage.Code != 0)
                return dataMessage;

            // All checks passed, continue creating a new policy in tblPolicy


            return dataMessage;
        }

        private DataMessage Validate(SelfRenewal renewal)
        {
            var dataMessage = new DataMessage();
            var context = new ImisDB();
            var insuree = new TblInsuree();

            //InsuranceNumberNotFound = 3007,
            insuree = context.TblInsuree
                                .Where(i => i.Chfid == renewal.InsuranceNumber && i.ValidityTo == null)
                                .Include(i => i.Family).ThenInclude(f => f.TblPolicy).ThenInclude(p => p.Prod).ThenInclude(pr => pr.ConversionProd)
                                .FirstOrDefault();
            if (insuree == null)
            {
                dataMessage.Code = (int)Errors.Renewal.InsuranceNumberNotFound;
                dataMessage.MessageValue = "Insuree with given insurance number not found";
                dataMessage.ErrorOccured = true;
                return dataMessage;
            }

            //RenewalAlreadyRequested = 3008,
            if (insuree.Family.TblPolicy.Where(p => p.Prod.ProductCode == renewal.ProductCode && p.PolicyStatus == 1 && p.ValidityTo == null).FirstOrDefault() != null)
            {
                dataMessage.Code = (int)Errors.Renewal.RenewalAlreadyRequested;
                dataMessage.MessageValue = "Renewal already created";
                dataMessage.ErrorOccured = true;
                return dataMessage;
            }

            //NoPreviousPolicyFoundToRenew = 3009,
            var prevPolicy = insuree.Family.TblPolicy.Where(p => p.Prod.ProductCode == renewal.ProductCode  && p.ValidityTo == null).FirstOrDefault();
            var convPolicy = insuree.Family.TblPolicy.Where(p => p.Prod.ConversionProd.ProductCode == renewal.ProductCode  && p.ValidityTo == null).FirstOrDefault();
            if (prevPolicy == null && convPolicy == null)
            {
                dataMessage.Code = (int)Errors.Renewal.NoPreviousPolicyFoundToRenew;
                dataMessage.MessageValue = "No previous policy found with given renewal product";
                dataMessage.ErrorOccured = true;
                return dataMessage;
            }

            return dataMessage;
        }
    }
}
