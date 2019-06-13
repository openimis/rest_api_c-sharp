using Microsoft.EntityFrameworkCore;
using OpenImis.DB.SqlServer;
using OpenImis.Modules.CoverageModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenImis.Modules.CoverageModule.Repositories
{
    public class CoverageRepository : ICoverageRepository
    {
        public List<CoverageProduct> Get(string insureeNumber)
        {
            List<CoverageProduct> response = new List<CoverageProduct>();

            try
            {
                using (var imisContext = new ImisDB())
                {
                    response = imisContext.Query<CoverageProduct>().FromSql("uspAPIGetCoverage").ToList();
                }

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
