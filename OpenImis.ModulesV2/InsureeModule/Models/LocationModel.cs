using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.InsureeModule.Models
{
    public class LocationModel
    {
        public int LocationId { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public int? ParentLocationId { get; set; }
        public string LocationType { get; set; } /// TODO: this is char or string?
		public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public int LegacyId { get; set; }
        public int MalePopulation { get; set; }
        public int FemalePopulation { get; set; }
        public int OtherPopulation { get; set; }
        public int Families { get; set; }

        public ICollection<LocationModel> Locations { get; set; }
    }
}
