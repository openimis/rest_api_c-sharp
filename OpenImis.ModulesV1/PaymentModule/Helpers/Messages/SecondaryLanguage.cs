using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.PaymentModule.Helpers.Messages
{
    public static class SecondaryLanguage
    {
        public static string Success = "Succès";

        //CtrlNumberResponse
        public static string CantAssignCN = "1-Le numéro de contrôle ne peut pas être attribué par la passerelle de paiement externe";
        public static string DuplicateCN = "2-Numéro de contrôle en double";

        //DeleteMemberFamilyResponse
        public static string WrongFormatMissingIN = "Mauvais format ou numéro d'assurance manquant";
        public static string INNotFount = "Numéro d'assurance du membre non trouvé";
        public static string MemberNotHead = "Le membre est chef de famille";

        //EditFamilyResponse
        public static string WrongOrMissingHeadIN = "Mauvais format ou numéro d'assurance manquant";
        public static string HeadINNotFound = "Numéro d'assurance du chef de famille non trouvé";
        public static string WrongPVillageCode = "Mauvais code de village permanent";
        public static string WrongCVillageCode = "Code du village actuel incorrect";
        public static string WrongGender = "Mauvais genre";
        public static string WrongConfirmationType = "Mauvais type de confirmation";
        public static string WrongGroupType = "Mauvais type de groupe";
        public static string WrongMaritalStatus = "Mauvais type d'état matrimonial";
        public static string WrongEducation = "Mauvais type d'éducation";
        public static string WrongProfession = "Mauvais type de profession";
        public static string FSPCodeNotFound = "Code FSP non trouvé";
        public static string WrongIdentificationType = "Mauvais type d'identification";

        //EditMemberFamilyResponse
        public static string WrongINMember = "Mauvais format du numéro d'assurance du membre";
        public static string NotFountINMember = "Numéro d'assurance du membre non trouvé";
        public static string WrongVillageCode = "Code du village actuel incorrect";
        public static string WrongRelationship = "Mauvais type de relation";

        //EnterContributionResponse
        public static string WrongOrMissingPC = "Code de produit incorrect ou manquant (non existant ou non applicable à la famille / groupe)";
        public static string WrongOrMissingPayDate = "Date de paiement erronée ou manquante";
        public static string WrongContributionCat = "Mauvaise catégorie de contribution";
        public static string WrongOrMissingPayType = "Type de paiement incorrect ou manquant";
        public static string WrongOrMissingPayer = "Mauvais payeur ou manquant";
        public static string MissingReceiptNumber = "Numéro de reçu manquant";
        public static string DuplicateReceiptNumber = "Numéro de reçu dupliqué";

        //EnterFamilyResponse
        public static string DuplicateINHead = "Numéro d'assurance du chef de famille dupliqué";
        public static string WrongOrMissingPVC = "Code de village permanent incorrect ou manquant";
        public static string WrongOrMissingGender = "Genre incorrect ou manquant";
        public static string WrongFrOrMissingBd = "Mauvais format ou date de naissance manquante.";
        public static string MissingLastName = "Absence du nom de famille";
        public static string MissingOtherName = "Absence des prénoms";

        //EnterMemberFamilyResponse
        public static string DuplicatedMemberIN = "Numéro d'assurance du membre dupliqué";

        //EnterPolicyResponse
        public static string WrongOrMissingEnrolDate = "Date d'inscription incorrecte ou manquante";
        public static string WrongOrMissingEOcode = "Code du responsable d'inscription incorrect ou manquant (non existant ou non applicable à la famille / groupe)";

        //GetCoverageResponse

        //GetFamilyResponse

        //GetMemberFamilyResponse
        public static string NoMemberOfOrder = "Aucun membre du numéro de commande spécifié dans la famille / groupe";

        //MatchPayResponse
        public static string PayIdDoesntExist = "Le paymentId n'existe pas";

        //RenewPolicyResponse
        public static string WrongOrMissingRenDate = "Date de renouvellement incorrecte ou manquante";

        //RequestedCNResponse
        public static string WrongInternalIdFormat = "Mauvais format de l'identifiant interne";
        public static string InvalidInternalId = "Identifiant interne non valide";

        //SaveAckResponse
        public static string CantPostReq = "La demande de numéro de contrôle ne peut pas être envoyée dans le service de paiement externe";

        //SaveIntentResponse
        public static string WrongFormatInsureeNo = "1-Mauvais format du numéro d'assurance";
        public static string InValidINmissingPC = "2-Assurance non valide ou code de produit manquant";
        public static string InValidEOC = "3-Code du responsable d'inscription non valide";
        public static string IncompatibleEO_PC = "4-Le code du responsable d'inscription et le code de produit d'assurance ne sont pas compatibles";
        public static string NoRenewalProduct = "5-Le bénéficiaire n’a pas de police d’assurance spécifique pour le renouvellement";
        public static string InsureeNoMissing = "6-Numéro d'assurance manquant";
        public static string InsureeNotEnrolled = "7-Assuré non inscrit (inscription préalable obligatoire)";
        public static string DuplicateCNAssigned = "8-Numéro de contrôle dupliqué attribué";
        public static string CantAssignCn2 = "9-Le numéro de contrôle ne peut pas être attribué";
        public static string UnknownPaymentType = "10-Type de paiement inconnu";

        //SavePayResponse
        public static string WrongOrMissingRecDate = "1-Date de réception erronée ou manquante";
        public static string WrongFormatInputData = "2-Mauvais format des données d'entrée";
        public static string WrongControlNumber = "3-Mauvais numéro de contrôle";
        public static string WrongAmount = "4-Mauvais montant";
        public static string DuplicatePayAmount = "5-Montant du paiement dupliqué";
        public static string DoesntExistEO = "6-Le code du responsable d'inscription n'existe pas";
        public static string DoesntExistPC = "7-Le code produit n'existe pas";
        public static string NoPolicyForRenewal = "8-Le bénéficiaire n’a pas de police d’assurance spécifique pour le renouvellement";
        public static string UnknownTypeOfPay = "9-Type de paiement inconnu";
    }
}
