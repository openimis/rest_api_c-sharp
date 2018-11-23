using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.Utils.Models
{
	/// <summary>
	/// This class allows to add pagination information to the responses getting list of Entities
	/// </summary>
	/// TODO: think if there should be a string NextPage field with URL for next page (same for PreviousPage)
	public class PagerModel
	{
		public int CurrentPage { get; set; }
		public int ResultsPerPage { get; set; }
		public int TotalResults { get; set; }
		public int TotalPages { get; set; }
	}
}
