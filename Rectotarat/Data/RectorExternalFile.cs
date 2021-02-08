
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Rectotarat.Models;
using System.IO;
using System.Threading.Tasks;

namespace Rectotarat.Data
{
    public class RectorExternalFile
    {
        private IHostingEnvironment _environment;
        private IConfiguration _iconfiguration;
        public RectorExternalFile()
        {

        }
        public RectorExternalFile(IHostingEnvironment environment, IConfiguration iconfiguration)
        {
            _environment = environment;
            _iconfiguration = iconfiguration;

        }

        public async Task<Rector> UploadRectorWithPhoto(Rector rector, IFormFile upload)
        {
            string relativeFileName = "";
            string absoluteFileName = "";
            if (upload != null)
            {
                relativeFileName = _iconfiguration.GetSection("Paths").GetSection("PathToPhotos").Value + rector.RectorId.ToString() + upload.FileName;
                rector.Photo = relativeFileName;
                absoluteFileName = _environment.WebRootPath + relativeFileName;
                using (var fileStream = new FileStream(absoluteFileName, FileMode.Create))
                {
                    await upload.CopyToAsync(fileStream);
                }

            }
            return rector;
        }
 

    }
}

