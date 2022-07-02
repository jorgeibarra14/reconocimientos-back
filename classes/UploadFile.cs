using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Reconocimientos.Classes
{
    public class UploadFile
    {

        public string Upload(string folderName, string ext, IFormFile file, string rootPath) {
            
            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            string filename = "";
            string fullRoute = rootPath + "/Docs/" + folderName + "/";

            if (!Directory.Exists(fullRoute))
            {
                Directory.CreateDirectory(fullRoute);
            }
            
            if (file.Length > 0)
            {
                filename = timeStamp + '.' + ext.Split("/")[1];
        
                string RutaFullCompleta = Path.Combine(fullRoute, filename);
        
                using (var stream = new FileStream(RutaFullCompleta, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
        
            }



            return folderName + "/" +  filename;
        }

    }
}