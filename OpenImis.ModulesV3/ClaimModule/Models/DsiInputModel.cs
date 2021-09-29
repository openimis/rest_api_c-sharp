
using Newtonsoft.Json;
using OpenImis.ModulesV3.Helpers.Validators;
using OpenImis.ModulesV3.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ModulesV3.ClaimModule.Models
{
    public class DsiInputModel
    {
        [JsonConverter(typeof(IsoDateOnlyDatetimeSerializer))]
        public DateTime? last_update_date { get; set; }
    }
}
