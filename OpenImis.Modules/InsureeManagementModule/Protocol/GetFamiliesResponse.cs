using OpenImis.Modules.InsureeManagementModule.Models;
using OpenImis.Modules.Utils.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.InsureeManagementModule.Protocol
{
	public class GetFamiliesResponse
	{
		public PagerModel Pager { get; set; }
		public IEnumerable<FamilyModel> Families { get; set; }

		public GetFamiliesResponse()
		{
			Pager = new PagerModel();
			Families = new List<FamilyModel>();
		}

	}
}
