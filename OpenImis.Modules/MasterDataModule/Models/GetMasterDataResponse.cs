using System;

namespace OpenImis.Modules.MasterDataModule.Models
{
    public class GetMasterDataResponse
    {
        public LocationModel[] Locations { get; set; }
        public FamilyTypeModel[] FamilyTypes { get; set; }
        public ConfirmationTypeModel[] ConfirmationTypes { get; set; }
        public EducationLevelModel[] EducationLevels { get; set; }
        public GenderTypeModel[] GenderTypes { get; set; }
        public RelationTypeModel[] RelationTypes { get; set; }
        public ProfessionTypeModel[] ProfessionTypes { get; set; }
        public IdentificationTypeModel[] IdentificationTypes { get; set; }

    }
}
