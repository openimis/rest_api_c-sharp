using System;
using System.Collections.Generic;

namespace OpenImis.DB.SqlServer
{
    public partial class TblFamilies
    {
        public TblFamilies()
        {
            TblInsuree = new HashSet<TblInsuree>();
            TblPolicy = new HashSet<TblPolicy>();
            TblFamilySMS = new HashSet<TblFamilySMS>();
        }

        public int FamilyId { get; set; }
        public Guid FamilyUUID { get; set; }
        public int InsureeId { get; set; }
        public int? LocationId { get; set; }
        public bool? Poverty { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public byte[] RowId { get; set; }
        public string FamilyType { get; set; }
        public string FamilyAddress { get; set; }
        public bool? IsOffline { get; set; }
        public string Ethnicity { get; set; }
        public string ConfirmationNo { get; set; }
        public string ConfirmationType { get; set; }

        public TblConfirmationTypes ConfirmationTypeNavigation { get; set; }
        public TblFamilyTypes FamilyTypeNavigation { get; set; }
        public TblInsuree Insuree { get; set; }
        public TblLocations Location { get; set; }

        public ICollection<TblInsuree> TblInsuree { get; set; }
        public ICollection<TblPolicy> TblPolicy { get; set; }
        public ICollection<TblFamilySMS> TblFamilySMS { get; set; }
    }
}
