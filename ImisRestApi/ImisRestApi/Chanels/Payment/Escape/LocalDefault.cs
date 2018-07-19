using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Chanels.Payment.Escape
{
    public static class LocalDefault
    {
        public static object FamilyMambers()
        {
            return new { Adults = 2, Children = 3};
        }
    }
}
