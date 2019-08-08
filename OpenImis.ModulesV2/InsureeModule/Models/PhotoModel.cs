using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.InsureeModule.Models
{
    public class PhotoModel
    {
		public int PhotoId { get; set; }
        public InsureeModel Insuree { get; set; }
        public string CHFID { get; set; }
        public string PhotoFolder { get; set; }
        public string PhotoFileName { get; set; }
        public int OfficerId { get; set; }
        public DateTime PhotoDate { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
