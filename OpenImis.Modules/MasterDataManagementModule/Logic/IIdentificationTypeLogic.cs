using OpenImis.Modules.MasterDataManagementModule.Models;
using System.Threading.Tasks;

namespace OpenImis.Modules.MasterDataManagementModule.Logic
{
	public interface IIdentificationTypeLogic
	{
		Task<IdentificationTypeModel[]> GetAllIdentificationTypes();
	}
}
