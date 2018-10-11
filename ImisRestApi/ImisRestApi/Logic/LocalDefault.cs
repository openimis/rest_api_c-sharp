using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Logic
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
        public static FamilyDefaults FamilyMambers(IConfiguration config)
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

        public static bool ShouldSendSms(IConfiguration config,DateTime? lastSmsDate) {
            try
            {
                
                var value = Convert.ToInt32(config["Defaults:PeriodPayNotMatchedSms"]);
                if (value == 0)
                    return false;
                DateTime today = DateTime.UtcNow;
                DateTime thatday = (DateTime)lastSmsDate;

                int interval = (today - thatday).Days;

                if (lastSmsDate != null)
                {
                    if (value > 0)
                    {
                        if (interval / value >= 1 && lastSmsDate == null)
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
                else
                {
                    if (value > 0)
                    {
                        if (interval / value >= 1 && lastSmsDate == null)
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
    }
}
