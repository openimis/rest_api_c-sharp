using OpenImis.DB.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.MasterDataManagementModule.Models
{
	/// <summary>
	/// 
	/// </summary>
	/// TODO: add alternative language field
	/// TODO: change the table structure to have RelationTypeId, RelationType and language (EN, FR, etc.) 
	///			=> more than 2 languages 
	public class RelationTypeModel
	{
		public int RelationTypeId { get; set; }
		public string RelationType { get; set; }

		public static RelationTypeModel FromTblRelations(TblRelations tblRelations)
		{
			RelationTypeModel relationType = new RelationTypeModel()
			{
				RelationType = tblRelations.Relation,
				RelationTypeId = tblRelations.RelationId
			};

			return relationType;
		}

	}
}
