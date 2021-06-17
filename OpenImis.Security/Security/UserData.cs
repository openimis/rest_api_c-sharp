using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Security.Security
{
    public class UserData
    {
        public string UserID { get; set; }
        public Guid UserUUID { get; set; }
        public string LoginName { get; set; }
        public string PrivateKey { get; set; }
        public string StoredPassword { get; set; }
        public List<string> Rights { get; set; }

        public static explicit operator UserData(Models.UserData v)
        {
            return new UserData()
            {
                UserUUID = v.UserUUID,
                LoginName = v.LoginName,
                PrivateKey = v.PrivateKey
            };
        }

        //public static explicit operator UserData(Login.Models.UserData v)
        //{
        //    return new UserData()
        //    {
        //        UserUUID = v.UserUUID,
        //        LoginName = v.LoginName,
        //        PrivateKey = v.PrivateKey
        //    };
        //}

        //public static explicit operator UserData(ModulesV3.LoginModule.Models.UserData v)
        //{
        //    return new UserData()
        //    {
        //        UserUUID = v.UserUUID,
        //        LoginName = v.LoginName,
        //        PrivateKey = v.PrivateKey
        //    };
        //}
    }
}
