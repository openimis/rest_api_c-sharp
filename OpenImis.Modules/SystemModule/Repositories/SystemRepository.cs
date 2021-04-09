using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace OpenImis.Modules.SystemModule.Repositories
{
    public class SystemRepository : ISystemRepository
    {
        private IConfiguration _configuration;

        public SystemRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Get(string name)
        {
            string response;

            try
            {
                using (var imisContext = new ImisDB())
                {
                    var row = imisContext.TblImisdefaults.FirstOrDefault();
                    response = row.GetType().GetProperty(name).GetValue(row, null).ToString();
                }

                return response;
            }
            catch { }

            return null;
        }
    }
}