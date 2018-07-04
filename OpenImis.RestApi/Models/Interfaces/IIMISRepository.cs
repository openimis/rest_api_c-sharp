using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.RestApi.Models.Interfaces
{
    /// <summary>
    /// This interface serves to define a Service for the Entities repositories 
    /// </summary>
    public interface IIMISRepository
    {
        /// <summary>
        /// Return the user repository
        /// </summary>
        /// <returns>
        /// The instance of the user repository
        /// </returns>
        IUserRepository getUserRepository();
    }
}
