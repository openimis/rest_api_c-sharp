using System.Data;
using System.Data.SqlClient;

namespace OpenImis.ModulesV2.Helpers
{
    public static class OutputParameter
    {
        public static SqlParameter CreateOutputParameter(string parameterName, SqlDbType sqlDbType)
        {
            return new SqlParameter(parameterName, sqlDbType) { Direction = ParameterDirection.Output };
        }
    }
}
