using OpenImis.DB.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Security.Models
{
    public class UserData
    {
        public Guid UserUUID { get; set; }
        public string LoginName { get; set; }
        public string PrivateKey { get; set; }
        public string StoredPassword { get; set; }
        public List<string> Rights { get; set; }
    }


    public class Location
    {
        public int LocationId { get; set; }
        public string Code  { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
       
    }

    public class UserModel
    {
        public string LoginName { get; set; }
        public string EmailId { get; set; }
        public string OtherNames { get; set; }
        public string LastName { get; set; }
        public List<int> Rights { get; set; }

        public List<Location> Locations { get; set; }
    }

}
