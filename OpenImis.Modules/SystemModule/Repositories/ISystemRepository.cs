using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.SystemModule.Repositories
{
    public interface ISystemRepository
    {
        string Get(string name);
    }
}
