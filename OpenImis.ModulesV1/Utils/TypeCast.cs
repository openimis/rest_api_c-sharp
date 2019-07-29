using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ModulesV1.Utils
{
    public static class TypeCast
    {
		public static T Cast<T>(Object parentInstance)
		{
			T result = default(T);
			//try
			//{
				var serializedParent = JsonConvert.SerializeObject(parentInstance);
				result = JsonConvert.DeserializeObject<T>(serializedParent);
			//}
			//catch (Exception ex)
			//{
			//	return new T();
			//}
			return result;
		}

		public static T GetValue<T>(object value)
		{
			if (value == null || value == DBNull.Value)
				return default(T);
			else
				return (T)value;
		}
	}
}
