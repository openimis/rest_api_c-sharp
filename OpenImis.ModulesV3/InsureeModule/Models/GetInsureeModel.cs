using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using OpenImis.ModulesV3.Utils;

namespace OpenImis.ModulesV3.InsureeModule.Models
{
    public class GetInsureeModel
    {
        public string CHFID { get; set; }
        public string PhotoPath { get; set; }
        public string InsureeName { get; set; }
        [JsonConverter(typeof(IsoDateSerializer))]
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public string PhotoBase64 { get; set; }
        public string OtherNames { get; set; }
        public string LastName { get; set; }

    }
}
