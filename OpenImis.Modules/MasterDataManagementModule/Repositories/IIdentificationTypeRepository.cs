using OpenImis.Modules.MasterDataManagementModule.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenImis.Modules.MasterDataManagementModule.Repositories
{
	public interface IIdentificationTypeRepository
	{
		Task<IdentificationTypeModel[]> GetAllIdentificationTypes();
		Task<IdentificationTypeModel[]> GetIdentificationTypesByLanguage(string language);
	}
}
