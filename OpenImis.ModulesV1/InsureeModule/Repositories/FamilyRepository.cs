using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.DB.SqlServer.DataHelper;
using OpenImis.ModulesV1.InsureeModule.Helpers;
using OpenImis.ModulesV1.InsureeModule.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace OpenImis.ModulesV1.InsureeModule.Repositories
{
    public class FamilyRepository : IFamilyRepository
    {
        private IConfiguration Configuration;

        public int UserId { get; set; }

        public FamilyRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<FamilyModel> Get(string insureeNumber)
        {
            List<FamilyModel> response = new List<FamilyModel>();

            try
            {
                using (var imisContext = new ImisDB())
                {
                    response = (from F in imisContext.TblFamilies
                                join I in imisContext.TblInsuree on F.InsureeId equals I.InsureeId into tmpI
                                from I in tmpI.DefaultIfEmpty()
                                join L in imisContext.TblLocations on F.LocationId equals L.LocationId into tmpL
                                from L in tmpL.DefaultIfEmpty()
                                join C in imisContext.TblConfirmationTypes on F.ConfirmationType equals C.ConfirmationTypeCode into tmpC
                                from C in tmpC.DefaultIfEmpty()
                                join G in imisContext.TblFamilyTypes on F.FamilyType equals G.FamilyTypeCode into tmpG
                                from G in tmpG.DefaultIfEmpty()
                                join P in imisContext.TblProfessions on I.Profession equals P.ProfessionId into tmpP
                                from P in tmpP.DefaultIfEmpty()
                                join HF in imisContext.TblHf on I.Hfid equals HF.HfId into tmpHF
                                from HF in tmpHF.DefaultIfEmpty()
                                where (I.Chfid == insureeNumber
                                    && F.ValidityTo == null
                                    && I.ValidityTo == null
                                    && L.ValidityTo == null
                                    && HF.ValidityTo == null)
                                select new FamilyModel()
                                {
                                    InsuranceNumber = I.Chfid,
                                    OtherNames = I.OtherNames,
                                    LastName = I.LastName,
                                    BirthDate = I.Dob,
                                    Gender = I.Gender,
                                    PoveryStatus = F.Poverty,
                                    ConfirmationType = C.ConfirmationTypeCode,
                                    GroupType = F.FamilyType,
                                    PermanentAddress = F.FamilyAddress,
                                    MaritalStatus = I.Marital,
                                    BeneficiaryCard = I.CardIssued,
                                    CurrentVillageCode = L.LocationCode,
                                    CurrentAddress = I.CurrentAddress,
                                    Profession = P.Profession,
                                    Education = I.Education,
                                    PhoneNumber = I.Phone,
                                    Email = I.Email,
                                    IdentificationType = I.TypeOfId,
                                    IdentificationNumber = I.Passport,
                                    FSPCode = HF.Hfcode
                                })
                                .ToList();
                }

                return response;
            }
            catch (SqlException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<FamilyModelv2> GetMember(string insureeNumber, int order)
        {
            List<FamilyModelv2> response = new List<FamilyModelv2>();

            try
            {
                using (var imisContext = new ImisDB())
                {
                    var tblOrder = (from I in imisContext.TblInsuree
                                    join H in imisContext.TblInsuree.Where(a => a.ValidityTo == null
                                        && a.Chfid == insureeNumber
                                        && a.IsHead == true) on I.FamilyId equals H.FamilyId
                                    where I.ValidityTo == null
                                    orderby I.IsHead descending, I.InsureeId ascending
                                    select I.InsureeId)
                                        .ToArray()
                                        .Select((InsureeID, RowNo) => new
                                        {
                                            RowNo = RowNo + 1,
                                            InsureeID
                                        })
                                          .ToArray();

                    response = (from I in imisContext.TblInsuree
                                join O in tblOrder on I.InsureeId equals O.InsureeID
                                join F in imisContext.TblFamilies on I.FamilyId equals F.FamilyId
                                join L in imisContext.TblLocations on F.LocationId equals L.LocationId into tmpL
                                from L in tmpL.DefaultIfEmpty()
                                join C in imisContext.TblConfirmationTypes on F.ConfirmationType equals C.ConfirmationTypeCode into tmpC
                                from C in tmpC.DefaultIfEmpty()
                                join G in imisContext.TblFamilyTypes on F.FamilyType equals G.FamilyTypeCode into tmpG
                                from G in tmpG.DefaultIfEmpty()
                                join P in imisContext.TblProfessions on I.Profession equals P.ProfessionId into tmpP
                                from P in tmpP.DefaultIfEmpty()
                                join HF in imisContext.TblHf on I.Hfid equals HF.HfId into tmpHF
                                from HF in tmpHF.DefaultIfEmpty()
                                where (O.RowNo == order
                                    && F.ValidityTo == null
                                    && I.ValidityTo == null
                                    && L.ValidityTo == null
                                    && HF.ValidityTo == null)
                                select new FamilyModelv2
                                {
                                    InsuranceNumber = I.Chfid,
                                    OtherNames = I.OtherNames,
                                    LastName = I.LastName,
                                    BirthDate = I.Dob,
                                    Gender = I.Gender,
                                    PoveryStatus = F.Poverty,
                                    ConfirmationType = C.ConfirmationTypeCode,
                                    ConfirmationNo = F.ConfirmationNo,
                                    PermanentAddress = F.FamilyAddress,
                                    MaritalStatus = I.Marital,
                                    BeneficiaryCard = I.CardIssued,
                                    CurrentVillageCode = L.LocationCode,
                                    CurrentAddress = I.CurrentAddress,
                                    Profession = P.Profession,
                                    Education = I.Education,
                                    PhoneNumber = I.Phone,
                                    Email = I.Email,
                                    IdentificationType = I.TypeOfId,
                                    IdentificationNumber = I.Passport,
                                    FSPCode = HF.Hfcode
                                })
                                .ToList();
                }

                return response;
            }
            catch (SqlException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DataMessage AddNew(FamilyModelv3 model)
        {
            DataMessage message;

            try
            {
                SqlParameter[] sqlParameters = {
                    new SqlParameter("@AuditUserID", UserId),
                    new SqlParameter("@PermanentVillageCode", model.VillageCode),
                    new SqlParameter("@InsuranceNumber", model.HeadOfFamilyId),
                    new SqlParameter("@OtherNames", model.OtherName),
                    new SqlParameter("@LastName", model.LastName),
                    new SqlParameter("@BirthDate", model.BirthDate),
                    new SqlParameter("@Gender", model.Gender),
                    new SqlParameter("@PovertyStatus", model.PovertyStatus),
                    new SqlParameter("@ConfirmationType", model.ConfirmationType),
                    new SqlParameter("@GroupType", model.GroupType),
                    new SqlParameter("@ConfirmationNo", model.ConfrimationNo),
                    new SqlParameter("@PermanentAddress", model.PermanentAddressDetails),
                    new SqlParameter("@MaritalStatus", model.MaritalStatus),
                    new SqlParameter("@BeneficiaryCard", model.BeneficiaryCard),
                    new SqlParameter("@CurrentVillageCode", model.CurrentVillageCode),
                    new SqlParameter("@CurrentAddress", model.CurrentAddressDetails),
                    new SqlParameter("@Proffesion", model.Profession),
                    new SqlParameter("@Education", model.Education),
                    new SqlParameter("@PhoneNumber", model.PhoneNumber),
                    new SqlParameter("@Email", model.Email),
                    new SqlParameter("@IdentificationType", model.IdentificationType),
                    new SqlParameter("@IdentificationNumber", model.IdentificationNumber),
                    new SqlParameter("@FspCode", model.FspCode),
                };

                DataHelper helper = new DataHelper(Configuration);

                using (var imisContext = new ImisDB())
                {
                    var response = helper.Procedure("uspAPIEnterFamily", sqlParameters);

                    message = new EnterFamilyResponse(response.Code, false, response.Data, 0).Message;
                }

                return message;
            }
            catch (Exception e)
            {
                message = new EditFamilyResponse(e).Message;
            }
            return message;
        }

        public DataMessage Edit(EditFamily model)
        {
            DataHelper helper = new DataHelper(Configuration);

            DataMessage message;

            SqlParameter[] sqlParameters = {
                new SqlParameter("@AuditUserID", UserId),
                new SqlParameter("@InsuranceNumberOfHead", model.HeadOfFamilyId),
                new SqlParameter("@VillageCode", model.VillageCode),
                new SqlParameter("@OtherNames", model.OtherName),
                new SqlParameter("@LastName", model.LastName),
                new SqlParameter("@BirthDate", model.BirthDate),
                new SqlParameter("@Gender", model.Gender),
                new SqlParameter("@PovertyStatus", model.PovertyStatus),
                new SqlParameter("@ConfirmationType", model.ConfirmationType),
                new SqlParameter("@GroupType", model.GroupType),
                new SqlParameter("@ConfirmationNumber", model.ConfrimationNo),
                new SqlParameter("@PermanentAddress", model.PermanentAddressDetails),
                new SqlParameter("@MaritalStatus", model.MaritalStatus),
                new SqlParameter("@BeneficiaryCard", model.BeneficiaryCard),
                new SqlParameter("@CurrentVillageCode", model.CurrentVillageCode),
                new SqlParameter("@CurrentAddress", model.CurrentAddressDetails),
                new SqlParameter("@Proffesion", model.Profession),
                new SqlParameter("@Education", model.Education),
                new SqlParameter("@PhoneNumber", model.PhoneNumber),
                new SqlParameter("@Email", model.Email),
                new SqlParameter("@IdentificationType", model.IdentificationType),
                new SqlParameter("@IdentificationNumber", model.IdentificationNumber),
                new SqlParameter("@FSPCode", model.FspCode),
             };

            try
            {
                var response = helper.Procedure("uspAPIEditFamily", sqlParameters);
                message = new EditFamilyResponse(response.Code, false, response.Data, 0).Message;

                return message;
            }
            catch (Exception e)
            {
                message = new EditFamilyResponse(e).Message;
            }
            return message;
        }

        public DataMessage AddMember(FamilyMember model)
        {
            DataHelper helper = new DataHelper(Configuration);

            SqlParameter[] sqlParameters = {
                new SqlParameter("@AuditUserID", UserId),
                new SqlParameter("@InsureeNumber", model.InsureeNumber),
                new SqlParameter("@InsureeNumberOfHead", model.HeadInsureeNumber),
                new SqlParameter("@OtherNames", model.OtherName),
                new SqlParameter("@LastName", model.LastName),
                new SqlParameter("@BirthDate", model.BirthDate),
                new SqlParameter("@Gender", model.Gender),
                new SqlParameter("@Relationship", model.Relationship),
                new SqlParameter("@MaritalStatus", model.MaritalStatus),
                new SqlParameter("@BeneficiaryCard", model.Beneficiary_Card),
                new SqlParameter("@VillageCode", model.CurrentVillageCode),
                new SqlParameter("@CurrentAddress", model.CurrentAddressDetails),
                new SqlParameter("@Proffesion", model.Profession),
                new SqlParameter("@Education", model.Education),
                new SqlParameter("@PhoneNumber", model.PhoneNumber),
                new SqlParameter("@Email", model.Email),
                new SqlParameter("@IdentificationType", model.IdentificationType),
                new SqlParameter("@IdentificationNumber", model.IdentificationNumber),
                new SqlParameter("@FspCode", model.FspCode)
            };

            DataMessage message;

            try
            {
                var response = helper.Procedure("uspAPIEnterMemberFamily", sqlParameters);
                message = new EnterMemberFamilyResponse(response.Code, false, response.Data, 0).Message;
            }
            catch (Exception e)
            {

                message = new EnterMemberFamilyResponse(e).Message;
            }

            return message;
        }

        public DataMessage EditMember(EditFamilyMember model)
        {
            DataHelper helper = new DataHelper(Configuration);

            SqlParameter[] sqlParameters = {
                new SqlParameter("@AuditUserID", UserId),
                new SqlParameter("@InsureeNumber", model.InsureeNumber),
                new SqlParameter("@OtherNames", model.OtherName),
                new SqlParameter("@LastName", model.LastName),
                new SqlParameter("@BirthDate", model.BirthDate),
                new SqlParameter("@Gender", model.Gender),
                new SqlParameter("@Relationship", model.Relationship),
                new SqlParameter("@MaritalStatus",model.MaritalStatus),
                new SqlParameter("@BeneficiaryCard", model.Beneficiary_Card),
                new SqlParameter("@VillageCode", model.CurrentVillageCode),
                new SqlParameter("@CurrentAddress", model.CurrentAddressDetails),
                new SqlParameter("@Proffesion", model.Profession),
                new SqlParameter("@Education", model.Education),
                new SqlParameter("@PhoneNumber", model.PhoneNumber),
                new SqlParameter("@Email", model.Email),
                new SqlParameter("@IdentificationType", model.IdentificationType),
                new SqlParameter("@IdentificationNumber", model.IdentificationNumber),
                new SqlParameter("@FspCode", model.FspCode)
            };

            DataMessage message;

            try
            {
                var response = helper.Procedure("uspAPIEditMemberFamily", sqlParameters);
                message = new EditMemberFamilyResponse(response.Code, false, response.Data, 0).Message;
            }
            catch (Exception e)
            {

                message = new EditFamilyResponse(e).Message;
            }

            return message;
        }

        public DataMessage DeleteMember(string insureeNumber)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@InsuranceNumber", insureeNumber),
                new SqlParameter("@AuditUserID", UserId)
            };

            var data = new DataHelper(Configuration);

            DataMessage message;

            try
            {
                var response = data.Procedure("uspAPIDeleteMemberFamily", parameters);
                message = new DeleteMamberFamilyResponse(response.Code, false, response.Data, 0).Message;
            }
            catch (Exception e)
            {
                message = new DeleteMamberFamilyResponse(e).Message;
            }


            return message;
        }
    }
}