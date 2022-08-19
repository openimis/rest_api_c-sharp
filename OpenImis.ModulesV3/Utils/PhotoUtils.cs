using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace OpenImis.ModulesV3.Utils
{
    public static class PhotoUtils
    {
        public static string CreateBase64ImageFromFilepath(string photoPath,string imageName, ILogger logger=null)
        {
            var base64 = "";
            if (!string.IsNullOrEmpty(imageName))
            {
                var fileName = imageName.Replace('\\', '/').Split('/').Last();
                if (!string.IsNullOrEmpty(fileName)) 
                {
                    var fileFullPath = Path.Join(photoPath, fileName);
                    if (File.Exists(fileFullPath))
                    {
                        base64 = Convert.ToBase64String(File.ReadAllBytes(fileFullPath));
                    }
                }
            }
            return base64;
        }
    }
}
