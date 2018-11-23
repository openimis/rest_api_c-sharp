using OpenImis.Modules.MasterDataManagementModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.MasterDataManagementModule
{
	public class MasterDataManagementModule : IMasterDataManagementModule
	{
		protected readonly IImisModules imisModules;

		private ILocationLogic locationLogic;
		private IFamilyTypeLogic familyTypeLogic;
		private IConfirmationTypeLogic confirmationTypeLogic;
		private IEducationLevelLogic educationLevelLogic;
		private IGenderTypeLogic genderTypeLogic;
		private IRelationTypeLogic relationTypeLogic;
		private IProfessionTypeLogic professionTypeLogic;
		private IIdentificationTypeLogic identificationTypeLogic;

		public MasterDataManagementModule(IImisModules imisModules)
		{
			this.imisModules = imisModules;
		}

		public ILocationLogic GetLocationLogic()
		{
			if (locationLogic == null)
			{
				locationLogic = new LocationLogic(this.imisModules);
			}
			return locationLogic;
		}

		public IMasterDataManagementModule SetLocationLogic(ILocationLogic locationLogic)
		{
			this.locationLogic = locationLogic;
			return this;
		}

		public IFamilyTypeLogic GetFamilyTypeLogic()
		{
			if (familyTypeLogic == null)
			{
				familyTypeLogic = new FamilyTypeLogic(this.imisModules);
			}
			return familyTypeLogic;
		}

		public IMasterDataManagementModule SetFamilyTypeLogic(IFamilyTypeLogic familyTypeLogic)
		{
			this.familyTypeLogic = familyTypeLogic;
			return this;
		}

		public IConfirmationTypeLogic GetConfirmationTypeLogic()
		{
			if (confirmationTypeLogic == null)
			{
				confirmationTypeLogic = new ConfirmationTypeLogic(this.imisModules);
			}
			return confirmationTypeLogic;
		}

		public IMasterDataManagementModule SetConfirmationTypeLogic(IConfirmationTypeLogic confirmationTypeLogic)
		{
			this.confirmationTypeLogic = confirmationTypeLogic;
			return this;
		}

		public IEducationLevelLogic GetEducationLevelLogic()
		{
			if (educationLevelLogic == null)
			{
				educationLevelLogic = new EducationLevelLogic(this.imisModules);
			}
			return educationLevelLogic;
		}

		public IMasterDataManagementModule SetEducationLevelLogic(IEducationLevelLogic educationLeveLogic)
		{
			this.educationLevelLogic = educationLeveLogic;
			return this;
		}

		public IGenderTypeLogic GetGenderTypeLogic()
		{
			if (genderTypeLogic == null)
			{
				genderTypeLogic = new GenderTypeLogic(this.imisModules);
			}
			return genderTypeLogic;
		}

		public IMasterDataManagementModule SetGenderTypeLogic(IGenderTypeLogic genderTypeLogic)
		{
			this.genderTypeLogic = genderTypeLogic;
			return this;
		}

		public IRelationTypeLogic GetRelationTypeLogic()
		{
			if (relationTypeLogic == null)
			{
				relationTypeLogic = new RelationTypeLogic(this.imisModules);
			}
			return relationTypeLogic;
		}

		public IMasterDataManagementModule SetRelationTypeLogic(IRelationTypeLogic relationTypeLogic)
		{
			this.relationTypeLogic = relationTypeLogic;
			return this;
		}

		public IProfessionTypeLogic GetProfessionTypeLogic()
		{
			if (professionTypeLogic == null)
			{
				professionTypeLogic = new ProfessionTypeLogic(this.imisModules);
			}
			return professionTypeLogic;
		}

		public IMasterDataManagementModule SetProfessionTypeLogic(IProfessionTypeLogic professionTypeLogic)
		{
			this.professionTypeLogic = professionTypeLogic;
			return this;
		}

		public IIdentificationTypeLogic GetIdentificationTypeLogic()
		{
			if (identificationTypeLogic == null)
			{
				identificationTypeLogic = new IdentificationTypeLogic(this.imisModules);
			}
			return identificationTypeLogic;
		}

		public IMasterDataManagementModule SetIdentificationTypeLogic(IIdentificationTypeLogic identificationTypeLogic)
		{
			this.identificationTypeLogic = identificationTypeLogic;
			return this;
		}

	}
}
