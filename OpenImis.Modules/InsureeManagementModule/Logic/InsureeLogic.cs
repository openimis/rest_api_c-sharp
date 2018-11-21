using System.Threading.Tasks;
using OpenImis.Modules.InsureeManagementModule.Repositories;
using OpenImis.Modules.InsureeManagementModule.Models;

namespace OpenImis.Modules.InsureeManagementModule.Logic
{
	/// <summary>
	/// This class is actual implementation of IInsureeController methods for Master Version implementation 
	/// </summary>
	public class InsureeLogic: IInsureeLogic
    {

		protected readonly IInsureeRepository insureeRepository;
		protected readonly IImisModules imisModules;

        public InsureeLogic(IImisModules imisModules)
        {
			insureeRepository = new InsureeRepository();
			this.imisModules = imisModules;
        }

		/// <summary>
		/// Get insuree by insuree number
		/// </summary>
		/// <param name="insureeId"></param>
		/// <returns>InsureeModel</returns>
		public async Task<InsureeModel> GetInsureeByInsureeIdAsync(string insureeId)
		{
			// Validate input
			
			// Execute business behaviour
			InsureeModel insuree;
			insuree = await insureeRepository.GetInsureeByCHFIDAsync(insureeId);

			// Validate results

			// Validate data access rights

			// Return results 
			return insuree;
		}

	}
}
