using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.LoginModule.Models
{
    public class UserData
    {
        public string UserID { get; set; }
        public string LoginName { get; set; }
        public string PrivateKey { get; set; }
        public string StoredPassword { get; set; }
        public List<string> Rights { get; set; }
    }
}
