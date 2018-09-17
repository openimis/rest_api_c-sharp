using OpenImis.Modules.UserModule.Entities;
using OpenImis.Modules.WSModule.Models;
using OpenImis.RestApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.WSModule.Repositories
{
    /// <summary>
    /// This interface serves to define the methods which must be implemented in any specific instance 
    /// </summary>
    public interface IFamilyRepository
    {

		Task<FamilyModel> GetFamily(string chfId);

	}
}
