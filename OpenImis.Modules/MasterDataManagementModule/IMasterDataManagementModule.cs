using OpenImis.Modules.MasterDataManagementModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.MasterDataManagementModule
{
	public interface IMasterDataManagementModule
	{
		ILocationLogic GetLocationLogic();
		IMasterDataManagementModule SetLocationLogic(ILocationLogic locationLogic);

		IFamilyTypeLogic GetFamilyTypeLogic();
		IMasterDataManagementModule SetFamilyTypeLogic(IFamilyTypeLogic familyTypeLogic);

		IConfirmationTypeLogic GetConfirmationTypeLogic();
		IMasterDataManagementModule SetConfirmationTypeLogic(IConfirmationTypeLogic confirmationTypeLogic);

		IEducationLevelLogic GetEducationLevelLogic();
		IMasterDataManagementModule SetEducationLevelLogic(IEducationLevelLogic educationLeveLogic);

		IGenderTypeLogic GetGenderTypeLogic();
		IMasterDataManagementModule SetGenderTypeLogic(IGenderTypeLogic genderTypeLogic);

		IRelationTypeLogic GetRelationTypeLogic();
		IMasterDataManagementModule SetRelationTypeLogic(IRelationTypeLogic relationTypeLogic);

		IProfessionTypeLogic GetProfessionTypeLogic();
		IMasterDataManagementModule SetProfessionTypeLogic(IProfessionTypeLogic professionTypeLogic);

		IIdentificationTypeLogic GetIdentificationTypeLogic();
		IMasterDataManagementModule SetIdentificationTypeLogic(IIdentificationTypeLogic identificationTypeLogic);
	}
}
