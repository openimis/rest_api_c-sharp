using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.RestApi.Protocol
{
    /// <summary>
    /// This class serves as a parameter class for the Login call 
    /// </summary>
    public abstract class ARequestModel
    {
        private List<string> ErrorMessages { get; set; }

        private Boolean IsValidCalled = false;
        
    }
}
