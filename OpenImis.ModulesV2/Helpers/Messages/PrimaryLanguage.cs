using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.Helpers.Messages
{
    public static class PrimaryLanguage
    {
        public static string Success = "Success";
        public static string CantAssignCN = "1. Control number cannot be assigned by the external payment gateway.";
        public static string DuplicateCN = "2. Duplicate Control Number.";
        public static string WrongFormatMissingIN = "Wrong Format or Missing Insurance Number of Member.";
        public static string INNotFount = "Insurance number of member not found.";
        public static string MemberNotHead = "Member is not head of family.";
        public static string WrongOrMissingHeadIN = "Wrong Format or Missing Insurance Number of head.";
        public static string HeadINNotFound = "Insurance number of head not found.";
        public static string WrongPVillageCode = "Wrong permanent village code.";
        public static string WrongCVillageCode = "Wrong current village code.";
        public static string WrongGender = "Wrong gender.";
        public static string WrongConfirmationType = "Wrong confirmation type.";
        public static string WrongGroupType = "Wrong group type.";
        public static string WrongMaritalStatus = "Wrong marital status.";
        public static string WrongEducation = "Wrong education.";
        public static string WrongProfession = "Wrong profession.";
        public static string FSPCodeNotFound = "FSP code not found.";
        public static string WrongIdentificationType = "Wrong identification type.";
        public static string WrongINMember = "Wrong format of insurance number of member.";
        public static string NotFountINMember = "Insurance number of member not found.";
        public static string WrongVillageCode = "Wrong current village code.";
        public static string WrongRelationship = "Wrong relationship.";
        public static string WrongOrMissingPC = "Wrong or missing product code (not existing or not applicable to the family/group).";
        public static string WrongOrMissingPayDate = "Wrong or missing payment date.";
        public static string WrongContributionCat = "Wrong contribution category.";
        public static string WrongOrMissingPayType = "Wrong or missing payment type.";
        public static string WrongOrMissingPayer = "Wrong or missing payer.";
        public static string MissingReceiptNumber = "Missing receipt number.";
        public static string DuplicateReceiptNumber = "Duplicated receipt number.";
        public static string DuplicateINHead = "Duplicated Insurance Number of head.";
        public static string WrongOrMissingPVC = "Wrong or missing permanent village code.";
        public static string WrongOrMissingGender = "Wrong or missing gender.";
        public static string WrongFrOrMissingBd = "Wrong format or missing birth date.";
        public static string MissingLastName = "Missing last name.";
        public static string MissingOtherName = "Missing other name.";
        public static string DuplicatedMemberIN = "Insurance number of member duplicated.";
        public static string WrongOrMissingEnrolDate = "Wrong or missing enrollment date.";
        public static string WrongOrMissingEOcode = "Wrong or missing enrollment officer code (not existing or not applicable to the family/group).";
        public static string NoMemberOfOrder = "No member of the specified order number in the family/group.";
        public static string PayIdDoesntExist = "1. The payment identifier does not exist.";
        public static string WrongOrMissingRenDate = "Wrong or missing renewal date.";
        public static string WrongInternalIdFormat = "1. Wrong format of internal identifier.";
        public static string InvalidInternalId = "2. Not valid internal identifier.";
        public static string CantPostReq = "1. Request for control number cannot be posted in the external payment gateway.";
        public static string WrongFormatInsureeNo = "1. Wrong format of insurance number.";
        public static string InValidINmissingPC = "2. Not valid insurance or missing product code.";
        public static string InValidEOC = "3. Not valid enrollment officer code.";
        public static string IncompatibleEO_PC = "4. Enrollment officer code and insurance product code are not compatible.";
        public static string NoRenewalProduct = "5. Beneficiary has no policy of specified insurance product for renewal.";
        public static string InsureeNoMissing = "6. Missing insurance number.";
        public static string InsureeNotEnrolled = "7. Insuree not enrolled while prior enrollment mandatory.";
        public static string DuplicateCNAssigned = "8. Duplicated control number assigned.";
        public static string CantAssignCn2 = "9. Control number cannot be assigned.";
        public static string UnknownPaymentType = "10. Unknown type of payment.";
        public static string WrongOrMissingRecDate = "1. Wrong or missing receiving date.";
        public static string WrongFormatInputData = "2. Wrong format of input data.";
        public static string WrongControlNumber = "3. Wrong control number.";
        public static string WrongAmount = "4. Wrong amount.";
        public static string DuplicatePayAmount = "5. Duplicate Payment Amount.";
        public static string DoesntExistEO = "6. Enrollment Officer Code does not exist.";
        public static string DoesntExistPC = "7. Product Code Does not Exist.";
        public static string NoPolicyForRenewal = "8. Beneficiary has no policy of specified insurance product for renewal.";
        public static string UnknownTypeOfPay = "9. Unknown type of payment.";
    }
}
