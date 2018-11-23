using OpenImis.Modules.MasterDataManagementModule.Models;
using System.Threading.Tasks;

namespace OpenImis.Modules.MasterDataManagementModule.Repositories
{
	public interface IRelationTypeRepository
	{
		Task<RelationTypeModel[]> GetAllRelationTypes();
		Task<RelationTypeModel[]> GetRelationTypesByLanguage(string language);
	}
}
