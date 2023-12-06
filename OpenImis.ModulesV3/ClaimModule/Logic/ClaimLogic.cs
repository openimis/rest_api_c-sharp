﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenImis.DB.SqlServer;
using OpenImis.ModulesV3.ClaimModule.Models;
using OpenImis.ModulesV3.ClaimModule.Models.RegisterClaim;
using OpenImis.ModulesV3.ClaimModule.Repositories;
using System.Collections.Generic;

namespace OpenImis.ModulesV3.ClaimModule.Logic
{
    public class ClaimLogic
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILoggerFactory _loggerFactory;
        protected ClaimRepository claimRepository;

        public ClaimLogic(IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _loggerFactory = loggerFactory;

            claimRepository = new ClaimRepository(_configuration, _hostingEnvironment, _loggerFactory);
        }

        public List<SubmitClaimResponse> Create(List<Claim> claims)
        {
            int result;
            var claimResponse = new List<SubmitClaimResponse>();

            foreach (var claim in claims)
            {
                result = claimRepository.Create(claim);
                
                Errors.Claim errorCode;
                string message;
                switch (result)
                {
                    case 0:
                        errorCode = Errors.Claim.Success;
                        message = "Claim submitted successfully";
                        break;
                    case 1:
                        errorCode = Errors.Claim.InvalidHFCode;
                        message = "Health facility code does not exist";
                        break;
                    case 2:
                        errorCode = Errors.Claim.DuplicateClaimCode;
                        message = $"Claim code must be unique";
                        break;
                    case 3:
                        errorCode = Errors.Claim.InvalidInsuranceNumber;
                        message = $"Insurance number [{claim.Details.CHFID}] does not exist";
                        break;
                    case 4:
                        errorCode = Errors.Claim.EndDateIsBeforeStartDate;
                        message = "End date must be equal or greather than the start date";
                        break;
                    case 5:
                        errorCode = Errors.Claim.InvalidICDCode;
                        message = "Invalid ICD Code";
                        break;
                    case 7:
                        errorCode = Errors.Claim.InvalidItem;
                        message = "One or more Items are invalid";
                        break;
                    case 8:
                        errorCode = Errors.Claim.InvalidService;
                        message = "One or more services are invalid";
                        break;
                    case 9:
                        errorCode = Errors.Claim.InvalidClaimAdmin;
                        message = $"Claim administration code [{claim.Details.ClaimAdmin}] does not exist";
                        break;
                    case 2010:
                        errorCode = Errors.Claim.Rejected;
                        message = $"Claim rejected";
                        break;
                    case 2011:
                        errorCode = Errors.Claim.InvalidService;
                        message = "One or more Subservices are invalid";
                        break;
                    case 2012:
                        errorCode = Errors.Claim.InvalidService;
                        message = "One or more Subitems are invalid";
                        break;
                    default:
                        errorCode = Errors.Claim.UnexpectedException;
                        message = $"Unhandled exception occured ({result}). Please contact the system administrator";
                        break;
                }

                var rejectedItems = claimRepository.GetRejectedItems(claim.Details.HFCode, claim.Details.ClaimCode);
                var rejectedServices = claimRepository.GetRejectedServices(claim.Details.HFCode, claim.Details.ClaimCode);


                claimResponse.Add(
                    new SubmitClaimResponse
                    {
                        ClaimCode = claim.Details.ClaimCode,
                        Response = (int)errorCode,
                        Message = message,
                        RejectedItems = rejectedItems,
                        RejectedServices = rejectedServices
                    });

            }
            return claimResponse;
        }

        public DiagnosisServiceItem GetDsi(DsiInputModel model)
        {
            DiagnosisServiceItem message;

            message = claimRepository.GetDsi(model);

            return message;
        }

        public List<ClaimAdminModel> GetClaimAdministrators()
        {
            List<ClaimAdminModel> response;

            response = claimRepository.GetClaimAdministrators();

            return response;
        }

        public List<TblControls> GetControls()
        {
            List<TblControls> response;

            response = claimRepository.GetControls();

            return response;
        }

        public PaymentLists GetPaymentLists(PaymentListsInputModel model)
        {
            PaymentLists response;

            response = claimRepository.GetPaymentLists(model);

            return response;
        }

        public List<ClaimOutput> GetClaims(ClaimsModel model)
        {
            List<ClaimOutput> response;

            response = claimRepository.GetClaims(model);

            return response;
        }
    }
}
