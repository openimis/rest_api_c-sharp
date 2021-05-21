using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Logic
{
    public class FamilyDefaults
    {
        public int Adults { get; set; }
        public int Children { get; set; }
        public int OtherAdults { get; internal set; }
        public int OtherChildren { get; internal set; }
    }
    public static class LocalDefault
    {
        public static FamilyDefaults FamilyMembers(IConfiguration config)
        {
            try
            {
                var adults = Convert.ToInt32(config["Defaults:Family:Adults"]);
                var children = Convert.ToInt32(config["Defaults:Family:Children"]);
                var other_adults = Convert.ToInt32(config["Defaults:Family:OtherAdults"]);
                var other_children = Convert.ToInt32(config["Defaults:Family:OtherChildren"]);

                FamilyDefaults fam = new FamilyDefaults() { Adults = adults, Children = children,OtherAdults = other_adults,OtherChildren = other_children };
                return fam;
            }
            catch (Exception)
            {

                throw new Exception("A Family property is not properly defined in config file");
            }
            
        }

        public static bool PriorEnrolmentRequired(IConfiguration config)
        {
            try
            {
                var value = Convert.ToBoolean(config["Defaults:PriorEnrolment"]);
                return value;
            }
            catch (Exception)
            {
                throw new Exception("This property is not properly defined in config file");
            }
        }

        public static bool ShouldSendSms(IConfiguration config,DateTime? lastSmsDate, DateTime? matchedDate) {
            try
            {
                
                var value = Convert.ToInt32(config["Defaults:PeriodPayNotMatchedSms"]);
                if (value == 0)
                    return false;

                DateTime today = DateTime.UtcNow;

                if (lastSmsDate != null)
                {                 
                    DateTime thatday = (DateTime)lastSmsDate;
                    int interval = (today - thatday).Days;

                    if (value > 0)
                    {
                        return false;
                    }
                    else
                    {
                        value *= -1;
                        if (interval % value == 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }                       
                    }
                }
                else
                {
                    if (matchedDate != null)
                    {
                        DateTime thatday = (DateTime)matchedDate;
                        int interval = (today - thatday).Days;

                        if (interval / value >= 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                
            }
            catch (Exception)
            {
                throw new Exception("This property is not properly defined in config file");
            }
        }

        public static string[] PrimaryLanguageRepresentations(IConfiguration config)
        {
            List<string> langs = new List<string>();
            try
            {
                 var def_langs = config["Language:Primary"].Split(',');

                foreach (var def_lang in def_langs) {
                    try
                    {
                        langs.Add(def_lang.ToLower());
                    }
                    catch (Exception) {
                        langs.Add(def_lang);
                    }
                }
            }
            catch (Exception)
            {

                langs.Add("en");
            }

            return langs.ToArray();
        }
    }
}
