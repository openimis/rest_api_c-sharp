using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.RestApi.Protocol
{
    interface IRequestModel
    {
        Boolean IsValid();

        string[] GetErrorMessages();
    }
}
