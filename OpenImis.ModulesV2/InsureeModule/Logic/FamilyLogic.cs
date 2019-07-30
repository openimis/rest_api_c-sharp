using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace OpenImis.ModulesV2.InsureeModule.Logic
{
    public class FamilyLogic : IFamilyLogic
    {
        private IConfiguration _configuration;

        public FamilyLogic(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
