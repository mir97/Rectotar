
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Rectotarat.Models;
using System.IO;
using System.Threading.Tasks;

namespace Rectotarat.Data
{
    public class UniversityExternalFile
    {
        private IHostingEnvironment _environment;
        private IConfiguration _iconfiguration;
        public UniversityExternalFile()
        {

        }
        public UniversityExternalFile(IHostingEnvironment environment, IConfiguration iconfiguration)
        {
            _environment = environment;
            _iconfiguration = iconfiguration;

        }


        public async Task<University> UploadUniversityWithLogo(University university, IFormFile upload)
        {
            string relativeFileName = "";
            string absoluteFileName = "";
            if (upload != null)
            {
                relativeFileName = _iconfiguration.GetSection("Paths").GetSection("PathToLogos").Value + university.UniversityId.ToString() + upload.FileName;
                university.Logo = relativeFileName;
                absoluteFileName = _environment.WebRootPath + relativeFileName;
                using (var fileStream = new FileStream(absoluteFileName, FileMode.Create))
                {
                    await upload.CopyToAsync(fileStream);
                }

            }                    
            return university;
        }

       

    }
}
