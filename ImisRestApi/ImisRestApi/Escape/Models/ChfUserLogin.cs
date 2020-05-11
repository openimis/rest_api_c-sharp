using ImisRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Escape.Models
{
    public class ChfUserLogin : UserLogin
    {
        public string msisdn { get; set; }
        public int language { get; set; }
    }
}
