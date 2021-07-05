using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Responses.Messages
{
    public class Language
    {

        public string GetMessage(int language, string name)
        {
            FieldInfo fieldInfos;

            switch (language)
            {
                
                case 0:
                    fieldInfos = typeof(PrimaryLanguage).GetField(name);
                    break;
                case 1:
                    fieldInfos = typeof(SecondaryLanguage).GetField(name);
                   
                    break;
                default:
                    fieldInfos = typeof(PrimaryLanguage).GetField(name);
                    
                    break;
            }
            var val = (string)fieldInfos.GetValue(null);
            return val;
        }
        
    }
}
