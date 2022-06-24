using Newtonsoft.Json;
using OpenImis.DB.SqlServer;
using OpenImis.ModulesV3.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenImis.ModulesV3.InsureeModule.Models
{
    public class InsureeModel
    {
        public int InsureeId { get; set; }
        public Guid InsureeUUID { get; set; }
        public string IdentificationNumber { get; set; }
        public string CHFID { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        [JsonConverter(typeof(IsoDateSerializer))]
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string Marital { get; set; }
        public bool IsHead { get; set; }
        public string Phone { get; set; }
        public bool CardIssued { get; set; }
        public short? Relationship { get; set; }
        public short? Profession { get; set; }
        public short? Education { get; set; }
        public string Email { get; set; }
        public string TypeOfId { get; set; }
        public int? HFID { get; set; }
        public string CurrentAddress { get; set; }
        public string GeoLocation { get; set; }
        public string CurVillage { get; set; }
        public string PhotoPath { get; set; }
        public string IdentificationTypes { get; set; }
        public bool IsOffline { get; set; }
        public bool Vulnerability { get; set; }
        public string PhotoBase64 { get; set; }

        public static InsureeModel FromTblInsuree(TblInsuree tblInsuree)
        {
            if (tblInsuree == null)
            {
                return null;
            }

            InsureeModel insuree = new InsureeModel()
            {
                InsureeId = tblInsuree.InsureeId,
                InsureeUUID = tblInsuree.InsureeUUID,
                IdentificationNumber = tblInsuree.Passport,
                CHFID = tblInsuree.Chfid,
                LastName = tblInsuree.LastName,
                OtherNames = tblInsuree.OtherNames,
                DOB = (DateTime)tblInsuree.Dob,
                IsHead = tblInsuree.IsHead,
                Phone = tblInsuree.Phone,
                Gender = tblInsuree.Gender,
                Marital = tblInsuree.Marital,
                TypeOfId = tblInsuree.TypeOfId,
                CardIssued = tblInsuree.CardIssued,
                Email = tblInsuree.Email,
                CurrentAddress = tblInsuree.CurrentAddress,
                GeoLocation = tblInsuree.GeoLocation,
                IdentificationTypes = tblInsuree.TypeOfId,
                Vulnerability = tblInsuree.Vulnerability
            };

            if (tblInsuree.Relationship != null)
            {
                insuree.Relationship = (short)tblInsuree.Relationship;
			}

            if (tblInsuree.Profession != null)
            {
                insuree.Profession = TypeCast.GetValue<short>(tblInsuree.Profession);
			}

            if (tblInsuree.Education != null)
            {
                insuree.Education = TypeCast.GetValue<short>(tblInsuree.Education);
			}

            if (tblInsuree.Hfid != null)
            {
                insuree.HFID = (short)tblInsuree.Hfid;
            }

            if (tblInsuree.CurrentVillage != null)
            {
                insuree.CurVillage = tblInsuree.CurrentVillage.ToString();
            }

            if (tblInsuree.IsOffline != null)
            {
                insuree.IsOffline = (bool)tblInsuree.IsOffline;
            }

            if (tblInsuree.Photo != null)
            {
                insuree.PhotoPath = System.IO.Path.Combine(
                    tblInsuree.Photo.PhotoFolder != null ? tblInsuree.Photo.PhotoFolder : "",
                    tblInsuree.Photo.PhotoFileName
                );
            }

            return insuree;
        }
    }
}
