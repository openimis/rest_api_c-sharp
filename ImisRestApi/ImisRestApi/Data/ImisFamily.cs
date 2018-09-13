using ImisRestApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;
using ImisRestApi.Responses;
using Microsoft.Extensions.Configuration;

namespace ImisRestApi.Data
{
    public class ImisFamily
    {
        private IConfiguration Configuration;

        public ImisFamily(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public DataTable Get(string insureeNumber)
        {
            var sSQL = @"SELECT I.CHFID InsuranceNumber, I.OtherNames, I.LastName, I.DOB BirthDate, I.Gender, F.Poverty PoveryStatus, C.ConfirmationTypeCode ConfirmationType, F.ConfirmationNo ConfirmationNo, F.FamilyAddress PermanentAddress, I.Marital MaritalStatus, I.CardIssued BeneficiaryCard, l.LocationCode CurrentVillageCode, I.CurrentAddress CurrentAddress, P.Profession, I.Education,  I.Phone PhoneNumber,I.Email, I.TypeOfId IdentificationType, I.passport IdentificationNumber, HF.HFCode FSPCode  FROM tblFamilies F 
                            LEFT OUTER JOIN tblInsuree I ON F.InsureeID = I.InsureeID
                            LEFT OUTER JOIN tblLocations L ON L.LocationId = F.LocationId
                            LEFT OUTER JOIN tblConfirmationTypes C ON C.ConfirmationTypeCode =F.ConfirmationType
                            LEFT OUTER JOIN tblFamilyTypes G ON G.FamilyTypeCode= F.FamilyType
                            LEFT OUTER JOIN tblProfessions P ON P.ProfessionId= I.Profession
                            LEFT OUTER JOIN tblHF HF ON HF.HfID=I.HFID
                            WHERE 
                            I.CHFID=@CHFID
                            AND F.ValidityTo IS NULL
                            AND I.ValidityTo IS NULL
                            AND L.ValidityTo IS NULL
                            AND HF.ValidityTo IS NULL
                            ";

            SqlParameter[] parameters = {
                new SqlParameter("@CHFID", insureeNumber)
            };

            var data = new DataHelper(Configuration);
            var dt = new DataTable();

            try
            {
                 dt = data.GetDataTable(sSQL, parameters, CommandType.Text);

            }
            catch(SqlException e)
            {
                throw e;
            }
            catch (Exception e)
            {

                throw e;
            }
            
            return dt;
        }

        public DataTable GetMamber(string insureeNumber, int order)
        {
            var sSQL = @";WITH tblOrder AS
                        (
                        SELECT ROW_NUMBER() OVER(ORDER BY I.IsHead DESC, I.InsureeID ASC) AS RowNo, I.InsureeID FROM tblInsuree I 
                        INNER JOIN (SELECT FamilyID FROM tblInsuree WHERE ValidityTo IS NULL AND CHFID = @InsuranceNumberOfHead AND IsHead =1) H ON H.FamilyID=I.FamilyID
                        WHERE I.ValidityTo IS NULL 
                        )

                        SELECT I.CHFID InsuranceNumber, I.OtherNames, I.LastName, I.DOB BirthDate, I.Gender, F.Poverty PoveryStatus, C.ConfirmationTypeCode ConfirmationType, F.ConfirmationNo ConfirmationNo, F.FamilyAddress PermanentAddress, I.Marital MaritalStatus, I.CardIssued BeneficiaryCard, l.LocationCode CurrentVillageCode, I.CurrentAddress CurrentAddress, P.Profession, I.Education,  I.Phone PhoneNumber,I.Email, I.TypeOfId IdentificationType, I.passport IdentificationNumber, HF.HFCode FSPCode  FROM tblInsuree I 
                        INNER JOIN tblOrder O ON O.InsureeID = I.InsureeID
                        INNER JOIN tblFamilies F ON F.FamilyID = I.FamilyID
                        LEFT OUTER JOIN tblLocations L ON L.LocationId = F.LocationId
                        LEFT OUTER JOIN tblConfirmationTypes C ON C.ConfirmationTypeCode =F.ConfirmationType
                        LEFT OUTER JOIN tblFamilyTypes G ON G.FamilyTypeCode= F.FamilyType
                        LEFT OUTER JOIN tblProfessions P ON P.ProfessionId= I.Profession
                        LEFT OUTER JOIN tblHF HF ON HF.HfID=I.HFID
                        WHERE 
                        O.RowNo = @OrderNumber
                        AND F.ValidityTo IS NULL
                        AND I.ValidityTo IS NULL
                        AND L.ValidityTo IS NULL
                        AND HF.ValidityTo IS NULL";

            SqlParameter[] parameters = {
                new SqlParameter("@InsuranceNumberOfHead", insureeNumber),
                 new SqlParameter("@OrderNumber", order)
            };

            var data = new DataHelper(Configuration);
           
            var dt = new DataTable();

            try
            {
                dt = data.GetDataTable(sSQL, parameters, CommandType.Text);

            }
            catch (SqlException e)
            {
                throw e;
            }
            catch (Exception e)
            {

                throw e;
            }
            return dt;
        }
        public DataTable Enquire(string insureeNumber)
        {
            SqlParameter LocationId = new SqlParameter();
            LocationId.Value = 0;
            LocationId.SqlDbType = SqlDbType.Int;
            LocationId.ParameterName = "@LocationId";

            //SqlParameter[] parameters = {
            //    new SqlParameter("@CHFID", insureeNumber),
            //    new SqlParameter("@LocationId",0)
            //};
            SqlParameter[] parameters = {
                new SqlParameter("@CHFID", insureeNumber),
                LocationId
            };

            var data = new DataHelper(Configuration);

            var dt = new DataTable();

            try
            {
                dt = data.GetDataTable("uspPolicyInquiry", parameters, CommandType.StoredProcedure);
               
            }
            catch (SqlException e)
            {
                throw e;
            }
            catch (Exception e)
            {

                throw e;
            }
            return dt;
        }

        public DataMessage DeleteMamber(string insureeNumber)
        {

            SqlParameter[] parameters = {
                new SqlParameter("@InsuranceNumber", insureeNumber)
            };

            var data = new DataHelper(Configuration);
           
            var dt = new DataTable();

            DataMessage message;

            try
            {
                var response = data.Procedure("uspAPIDeleteMemberFamily", parameters);
                message = new EditFamilyResponse(response, false).Message;
            }
            catch (Exception e)
            {

                message = new EditFamilyResponse(e).Message;
            }


            return message;

        }

        public DataMessage AddNew(Family model)
        {

            DataHelper helper = new DataHelper(Configuration);

            SqlParameter[] sqlParameters = {
                new SqlParameter("@PermanentVillageCode", model.VillageCode),
                new SqlParameter("@InsuranceNumber", model.HeadOfFamilyId),
                new SqlParameter("@OtherNames", model.OtherName),
                new SqlParameter("@LastName", model.LastName),
                new SqlParameter("@BirthDate", model.BirthDate),
                new SqlParameter("@Gender", new GenderValue(model.Gender).Value),
                new SqlParameter("@PovertyStatus", model.PovertyStatus),
                new SqlParameter("@ConfirmationType", model.ConfirmationType),
                new SqlParameter("@GroupType", model.GroupType),
              //  new SqlParameter("@ConfrimationNo", model.ConfrimationNo),
                new SqlParameter("@PermanentAddress", model.PermanentAddressDetails),
                new SqlParameter("@MaritalStatus", new MaritalStatusVal(model.MaritalStatus).Value),
                new SqlParameter("@BeneficiaryCard", model.BeneficiaryCard),
                new SqlParameter("@CurrentVillageCode", model.CurrentVillageCode),
                new SqlParameter("@CurrentAddress", model.CurrentAddressDetails),
                new SqlParameter("@Proffesion", model.Profession),
                new SqlParameter("@Education", model.Education),
                new SqlParameter("@PhoneNumber", model.PhoneNumber),
                new SqlParameter("@Email", model.Email),
               // new SqlParameter("@IdentificationType", model.IdentificationType),
                new SqlParameter("@IdentificationNumber", model.IdentificationNumber),
                new SqlParameter("@FspCode", model.FspCode),
            };

           
            DataMessage message;

            try
            {
                var response = helper.Procedure("uspAPIEnterFamily", sqlParameters);
                message = new EnterFamilyResponse(response, false).Message;

            }
            catch (Exception e)
            {

                message = new EditFamilyResponse(e).Message;
            }

            return message;
        }

        public DataMessage Edit(Family model)
        {

            DataHelper helper = new DataHelper(Configuration);

            SqlParameter[] sqlParameters = {
                new SqlParameter("@InsuranceNumberOfHead", model.HeadOfFamilyId),
                //new SqlParameter("@LocationCode", model.HeadOfFamilyId),
                new SqlParameter("@OtherNames", model.OtherName),
                new SqlParameter("@LastName", model.LastName),
                new SqlParameter("@BirthDate", model.BirthDate),
                new SqlParameter("@Gender", new GenderValue(model.Gender).Value),
                new SqlParameter("@PovertyStatus", model.PovertyStatus),
                new SqlParameter("@ConfirmationType", model.ConfirmationType),
               // new SqlParameter("@GroupType", model.GroupType),
               // new SqlParameter("@ConfrimationNo", model.ConfrimationNo),
                new SqlParameter("@PermanentAddress", model.PermanentAddressDetails),
                new SqlParameter("@MaritalStatus", new MaritalStatusVal(model.MaritalStatus).Value),
                new SqlParameter("@BeneficiaryCard", model.BeneficiaryCard),
                new SqlParameter("@VillageCode", model.CurrentVillageCode),
                new SqlParameter("@CurrentAddress", model.CurrentAddressDetails),
                new SqlParameter("@Proffesion", model.Profession),
                new SqlParameter("@Education", model.Education),
                new SqlParameter("@PhoneNumber", model.PhoneNumber),
                new SqlParameter("@Email", model.Email),
               // new SqlParameter("@IdentificationType", model.IdentificationType),
                new SqlParameter("@IdentificationNumber", model.IdentificationNumber),
                new SqlParameter("@FSPCode", model.FspCode),
            };

            DataMessage message;

            try
            {
                var response = helper.Procedure("uspAPIEditFamily", sqlParameters);
                message = new EditFamilyResponse(response, false).Message;
            }
            catch (Exception e)
            {

                message = new EditFamilyResponse(e).Message;
            }
            

            return message;
        }

        public DataMessage AddMamber(FamilyMamber model)
        {
            DataHelper helper = new DataHelper(Configuration);

            SqlParameter[] sqlParameters = {
                new SqlParameter("@InsureeNumber", model.InsureeNumber),
                new SqlParameter("@InsureeNumberOfHead", model.HeadInsureeNumber),
                new SqlParameter("@OtherNames", model.OtherName),
                new SqlParameter("@LastName", model.LastName),
                new SqlParameter("@BirthDate", model.BirthDate),
                new SqlParameter("@Gender", new GenderValue(model.Gender).Value),
                new SqlParameter("@Relationship", model.Relationship),
                new SqlParameter("@MaritalStatus", new MaritalStatusVal(model.MaritalStatus).Value),
                new SqlParameter("@BeneficiaryCard", model.Beneficiary_Card),
                new SqlParameter("@VillageCode", model.CurrentVillageCode),
                new SqlParameter("@CurrentAddress", model.CurrentAddressDetails),
                new SqlParameter("@Proffesion", model.Profession),
                new SqlParameter("@Education", model.Education),
                new SqlParameter("@PhoneNumber", model.PhoneNumber),
                new SqlParameter("@Email", model.Email),
               // new SqlParameter("@IdentificationType", model.IdentificationType),
                new SqlParameter("@IdentificationNumber", model.IdentificationNumber),
                new SqlParameter("@FspCode", model.FspCode)
            };

           
            DataMessage message;

            try
            {
                var response = helper.Procedure("uspAPIEnterMemberFamily", sqlParameters);
                message = new EnterMemberFamilyResponse(response,false).Message;
            }
            catch (Exception e)
            {

                message = new EnterMemberFamilyResponse(e).Message;
            }

            return message;
        }

        public DataMessage EditMamber(FamilyMamber model)
        {
            DataHelper helper = new DataHelper(Configuration);

            SqlParameter[] sqlParameters = {
                new SqlParameter("@InsureeNumber", model.InsureeNumber),
               // new SqlParameter("@InsureeNumberOfHead", model.HeadInsureeNumber),
                new SqlParameter("@OtherNames", model.OtherName),
                new SqlParameter("@LastName", model.LastName),
                new SqlParameter("@BirthDate", model.BirthDate),
                new SqlParameter("@Gender", new GenderValue(model.Gender).Value),
               // new SqlParameter("@Relationship", model.Relationship),
                new SqlParameter("@MaritalStatus", new MaritalStatusVal(model.MaritalStatus).Value),
                new SqlParameter("@BeneficiaryCard", model.Beneficiary_Card),
                new SqlParameter("@VillageCode", model.CurrentVillageCode),
                new SqlParameter("@CurrentAddress", model.CurrentAddressDetails),
                new SqlParameter("@Proffesion", model.Profession),
                new SqlParameter("@Education", model.Education),
                new SqlParameter("@PhoneNumber", model.PhoneNumber),
                new SqlParameter("@Email", model.Email),
               // new SqlParameter("@IdentificationType", model.IdentificationType),
                new SqlParameter("@IdentificationNumber", model.IdentificationNumber),
                new SqlParameter("@FspCode", model.FspCode)
            };

            DataMessage message;

            try
            {
                var response = helper.Procedure("uspAPIEditMemberFamily", sqlParameters);
                message = new EditMemberFamilyResponse(response, false).Message;
            }
            catch (Exception e)
            {

                message = new EditFamilyResponse(e).Message;
            }

            return message;
        }
    }
}