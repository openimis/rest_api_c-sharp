using System;
using System.Collections.Generic;

namespace OpenImis.RestApi.Models.Entities
{
    public partial class TblLocations
    {
        public TblLocations()
        {
            TblBatchRun = new HashSet<TblBatchRun>();
            TblFamilies = new HashSet<TblFamilies>();
            TblHf = new HashSet<TblHf>();
            TblHfcatchment = new HashSet<TblHfcatchment>();
            TblOfficer = new HashSet<TblOfficer>();
            TblOfficerVillages = new HashSet<TblOfficerVillages>();
            TblPayer = new HashSet<TblPayer>();
            TblPlitems = new HashSet<TblPlitems>();
            TblPlservices = new HashSet<TblPlservices>();
            TblProduct = new HashSet<TblProduct>();
            TblUsersDistricts = new HashSet<TblUsersDistricts>();
        }

        public int LocationId { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public int? ParentLocationId { get; set; }
        public string LocationType { get; set; }
        public DateTime? ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int? AuditUserId { get; set; }
        public byte[] RowId { get; set; }
        public int? MalePopulation { get; set; }
        public int? FemalePopulation { get; set; }
        public int? OtherPopulation { get; set; }
        public int? Families { get; set; }

        public ICollection<TblBatchRun> TblBatchRun { get; set; }
        public ICollection<TblFamilies> TblFamilies { get; set; }
        public ICollection<TblHf> TblHf { get; set; }
        public ICollection<TblHfcatchment> TblHfcatchment { get; set; }
        public ICollection<TblOfficer> TblOfficer { get; set; }
        public ICollection<TblOfficerVillages> TblOfficerVillages { get; set; }
        public ICollection<TblPayer> TblPayer { get; set; }
        public ICollection<TblPlitems> TblPlitems { get; set; }
        public ICollection<TblPlservices> TblPlservices { get; set; }
        public ICollection<TblProduct> TblProduct { get; set; }
        public ICollection<TblUsersDistricts> TblUsersDistricts { get; set; }
    }
}
