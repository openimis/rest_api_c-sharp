using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.MasterDataModule.Models
{
    public class MasterDataModel
    {
        public List<ConfirmationTypeModel> ConfirmationTypes { get; set; }
        public List<ControlModel> Controls { get; set; }
        public List<EducationLevelModel> Education { get; set; }
        public List<FamilyTypeModel> FamilyTypes { get; set; }
        public List<HFModel> HF { get; set; }
        public List<IdentificationTypeModel> IdentificationTypes { get; set; }
        public List<LanguageModel> Languages { get; set; }
        public List<LocationModel> Locations { get; set; }
        public List<OfficerModel> Officers { get; set; }
        public List<PayerModel> Payers { get; set; }
        public List<ProductModel> Products { get; set; }
        public List<ProfessionTypeModel> Professions { get; set; }
        public List<RelationTypeModel> Relations { get; set; }
        public List<PhoneDefaultModel> PhoneDefaults { get; set; }
        public List<GenderTypeModel> Genders { get; set; }
    }
}
