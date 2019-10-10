using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.InsureeModule.Models
{
    public class GetInsureeModel
    {
        public string CHFID { get; set; }
        public string PhotoPath { get; set; }
        public string InsureeName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
    }
}
