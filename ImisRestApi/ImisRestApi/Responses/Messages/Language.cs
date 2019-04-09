using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ImisRestApi.Responses.Messages
{
    public class Language
    {

        public string GetMessage(int language, string name)
        {
            FieldInfo fieldInfos;

            switch (language)
            {
                
                case 0:
                    fieldInfos = typeof(PrimaryLanguage).GetField("Success");
                    break;
                case 1:
                    fieldInfos = typeof(SecondaryLanguage).GetField("Success");
                   
                    break;
                default:
                    fieldInfos = typeof(PrimaryLanguage).GetField("Success");
                    
                    break;
            }
            var val = (string)fieldInfos.GetValue(null);
            return val;
        }
        
    }
}
