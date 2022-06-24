using System;
using System.Collections.Generic;
using System.IO;
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
                var startIndex = imageName.LastIndexOf("\\") == -1 ? 0 : imageName.LastIndexOf("\\");
                var fileName = imageName.Substring(startIndex);
                var fileFullPath =System.IO.Path.Combine(photoPath, imageName);

                if (File.Exists(fileFullPath))
                {
                    base64 = Convert.ToBase64String(File.ReadAllBytes(fileFullPath));
                }
            }
            return base64;
        }
    }
}
