using Rectotarat.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;


namespace Rectotarat.Data
{
    public class DocumentExternalFile
    {
        private IHostingEnvironment _environment;
        private IConfiguration _iconfiguration;
        public DocumentExternalFile()
        {

        }
        public DocumentExternalFile(IHostingEnvironment environment, IConfiguration iconfiguration)
        {
            _environment = environment;
            _iconfiguration = iconfiguration;

        }


        public async Task<Document> UploadDocument(Document document, IFormFile upload)
        {
            string relativeFileName = "";
            string absoluteFileName = "";
            if (upload != null)
            {
                relativeFileName = _iconfiguration.GetSection("Paths").GetSection("PathToDocuments").Value + document.DocumentId.ToString() + upload.FileName;
                document.DocumentURL = relativeFileName;
                absoluteFileName = _environment.WebRootPath + relativeFileName;
                using (var fileStream = new FileStream(absoluteFileName, FileMode.Create))
                {
                    await upload.CopyToAsync(fileStream);
                }

            }
            return document;
        }

    }
}
