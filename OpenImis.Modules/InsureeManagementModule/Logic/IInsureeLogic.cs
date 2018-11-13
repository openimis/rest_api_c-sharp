using OpenImis.Modules.InsureeManagementModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.InsureeManagementModule.Logic
{
    /// <summary>
    /// This interface serves to define the methods which must be implemented in any specific instance 
    /// </summary>
    public interface IInsureeLogic
    {

		/// <summary>
		/// Get insuree by insuree number
		/// </summary>
		/// <param name="insureeId"></param>
		/// <returns>InsureeModel</returns>
		Task<InsureeModel> GetInsureeById(string insureeId);
				
	}
}
