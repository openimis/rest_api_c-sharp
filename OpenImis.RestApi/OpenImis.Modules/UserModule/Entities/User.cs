using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using OpenImis.DB.SqlServer;

namespace OpenImis.Modules.UserModule.Entities
{
    public partial class User: TblUsers
	{
        //public User():base()
        //{
            
        //}

       
        private readonly Dictionary<int, string> rolesMapping = new Dictionary<int, string>
        {
            {1, "EnrollmentOfficer"},
            {2, "Manager"},
            {4, "Accountant"},
            {8, "Clerk"},
            {16, "MedicalOfficer"},
            {32, "SchemeAdmin"},
            {64, "IMISAdmin" },
            {128, "Receptionist"},
            {256, "ClaimAdmin"},
            {512, "ClaimContrib"},
            {524288, "HFAdmin"},
            {1048576, "OfflineSchemeAdmin"}
        };

        public bool CheckPassword(string password)
        {
            if (this.Password.ToString() == password)
            {
                return true;
            }

            return false;
        }

        public String[] GetRolesStringArray()
        {
            var roles = new List<String> { };
            foreach (KeyValuePair<int, string> role in rolesMapping)
            {
                if ((this.RoleId & role.Key) == role.Key)
                {
                    roles.Add(role.Value);
                }
            }
            
            return roles.ToArray();
        }

        public String NewPrivateKey()
        {
            this.PrivateKey = Guid.NewGuid().ToString();
            return this.PrivateKey;
        }
    }
}
