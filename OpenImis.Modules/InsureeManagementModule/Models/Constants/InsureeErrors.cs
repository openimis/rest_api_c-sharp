using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.InsureeManagementModule.Models.Constants
{
	/// <summary>
	/// 
	/// </summary>
	/// TODO: should we separate the errors by entity or by process?
	public static class InsureeErrors
	{
		public const string MISSING_INSUREE_NUMBER_ERROR = "0:The insuree number is missing";
		public const string INSUREE_NUMBER_EXISTS_ERROR = "1:The insuree number is already registered";
	}
}
