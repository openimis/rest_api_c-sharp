using OpenImis.Modules.WSModule.Models;
using OpenImis.RestApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.WSModule.Controllers
{
    /// <summary>
    /// This interface serves to define the methods which must be implemented in any specific instance 
    /// </summary>
    public interface IInsureeController
    {

		/// <summary>
		/// Get insuree by insuree number
		/// </summary>
		/// <param name="chfId"></param>
		/// <returns>InsureeModel</returns>
		Task<InsureeModel> GetInsuree(string chfId);
				
	}
}
