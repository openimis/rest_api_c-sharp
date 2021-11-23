using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Responses.Messages
{
    public static class SecondaryLanguage
    {


        public static string Success = "Succès";
        public static string CantAssignCN = "1. Le numéro de contrôle ne peut être assigné par le portail de paiement externe.";
        public static string DuplicateCN = "2. Le numéro de contrôle déjà utilisé.";
        public static string WrongFormatMissingIN = "Le numéro d’assuré a un format incorrect ou est manquant.";
        public static string INNotFount = "Numéro d’assuré du membre introuvable.";
        public static string MemberNotHead = "Le membre n'est pas le chef de famille.";
        public static string WrongOrMissingHeadIN = "Numéro d’assuré du chef de famille a un mauvais format ou est manquant.";
        public static string HeadINNotFound = "Numéro d’assuré du chef de famille est introuvable.";
        public static string WrongPVillageCode = "Le code du village permanent  incorrect.";
        public static string WrongCVillageCode = "Le code du village incorrect.";
        public static string WrongGender = "Mauvais genre.";
        public static string WrongConfirmationType = "Type de confirmation incorrect.";
        public static string WrongGroupType = "Type de groupe incorrect.";
        public static string WrongMaritalStatus = "Statut matrimonial incorrect.";
        public static string WrongEducation = "Éducation incorrecte.";
        public static string WrongProfession = "Profession incorrecte.";
        public static string FSPCodeNotFound = "Code FSP introuvable.";
        public static string WrongIdentificationType = "Type d’Identification incorrect.";
        public static string WrongINMember = "Mauvais format du numéro d’assuré du membre.";
        public static string NotFountINMember = "Numéro d’assuré de membre introuvable.";
        public static string WrongVillageCode = "Code du village erroné.";
        public static string WrongRelationship = "Relation erronée.";
        public static string WrongOrMissingPC = "Code du paquet de bénéfices erroné ou manquant (non existant ou sans objet pour la famille/groupe)";
        public static string WrongOrMissingPayDate = "Date de paiement erroné ou manquante.";
        public static string WrongContributionCat = "Catégorie de contribution erronée.";
        public static string WrongOrMissingPayType = "Type de paiement erroné ou manquant.";
        public static string WrongOrMissingPayer = "Donneur d’ordre erroné ou manquant.";
        public static string MissingReceiptNumber = "Numéro de reçu manquant.";
        public static string DuplicateReceiptNumber = "Numéro de reçu déjà utilisé.";
        public static string DuplicateINHead = "Numéro d'assuré du chef de famille déjà utilisé.";
        public static string WrongOrMissingPVC = "Code permanent du village manquant.";
        public static string WrongOrMissingGender = "Genre erroné ou manquant.";
        public static string WrongFrOrMissingBd = "Format de la date de naissance incorrect ou manquant.";
        public static string MissingLastName = "Nom de famille manquant.";
        public static string MissingOtherName = "Autre nom manquant.";
        public static string DuplicatedMemberIN = "Numéro d'assurance du membre déjà utilisé.";
        public static string WrongOrMissingEnrolDate = "Date d’inscription incorrecte ou absente.";
        public static string WrongOrMissingEOcode = "Code d’agent d'adhésion incorrecte ou absente (non existant ou non applicable à la famille/groupe)";
        public static string NoMemberOfOrder = "Aucun membre de l'ordre spécifié est dans cette famille/groupe.";
        public static string PayIdDoesntExist = "1. L'identifiant de payement n’existe pas.";
        public static string WrongOrMissingRenDate = "Date de renouvellement erroné ou manquante.";
        public static string WrongInternalIdFormat = "1. Mauvais format de l'identificateur interne.";
        public static string InvalidInternalId = "2. L'identificateur interne n’est pas valide.";
        public static string CantPostReq = "1. La demande de numéro de contrôle ne pas pu être envoyée à la passerelle de paiement externe.";
        public static string WrongFormatInsureeNo = "1. Mauvais format de numéro d’assurance.";
        public static string InValidINmissingPC = "2. Panier de soin non valide ou manquant.";
        public static string InValidEOC = "3. Code d'agent d'adhésion non valide.";
        public static string IncompatibleEO_PC = "4. Le code d'agent d'adhésion et le code du panier de soin ne sont pas compatibles.";
        public static string NoRenewalProduct = "5. Le bénéficiaire n’a aucune police d'assurance.pour le produit spécifié lors de la demande de renouvellement.";
        public static string InsureeNoMissing = "6. Numéro d’assuré manquant.";
        public static string InsureeNotEnrolled = "7. Inscription préalable de l'assuré obligatoire.";
        public static string DuplicateCNAssigned = "8. Numéro de contrôle attribué déjà utilisé.";
        public static string CantAssignCn2 = "9. Numéro de contrôle ne peut pas être assigné.";
        public static string UnknownPaymentType = "10. Type de paiement inconnue.";
        public static string WrongOrMissingRecDate = "1. Date de réception fausse ou manquante.";
        public static string WrongFormatInputData = "2. Mauvais format des données d'entrée.";
        public static string WrongControlNumber = "3. Numéro de contrôle incorrect.";
        public static string WrongAmount = "4. Montant incorrect.";
        public static string DuplicatePayAmount = "5. Montant du paiement en double.";
        public static string DoesntExistEO = "6. Le code de l'agent d'inscription n’existe pas.";
        public static string DoesntExistPC = "7. Le code du panier de soin n’existe pas.";
        public static string NoPolicyForRenewal = "8. Le bénéficiaire n’a aucune police d'assurance spécifiée pour le renouvellement du produit.";
        public static string UnknownTypeOfPay = "9. Type de paiement inconnu.";


    }
}
