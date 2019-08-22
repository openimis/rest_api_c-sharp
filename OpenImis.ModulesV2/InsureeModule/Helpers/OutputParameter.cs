using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace OpenImis.ModulesV2.InsureeModule.Helpers
{
    public static class OutputParameter
    {
        public static SqlParameter CreateOutputParameter(string parameterName, SqlDbType sqlDbType)
        {
            return new SqlParameter(parameterName, sqlDbType) { Direction = ParameterDirection.Output };
        }
    }
}
