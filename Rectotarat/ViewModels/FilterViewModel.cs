using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rectotarat.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rectotarat.ViewModels
{
    public class FilterViewModel
    {
        public FilterViewModel( string UniversityName,string LastName,string Email, string RegistrationNumber, string DocumentName)
        {


            SelectedUniversityName = UniversityName;
            SelectedLastName = LastName;
            SelectedEmail = Email;
            SelectedRegistrationNumber = RegistrationNumber;
            SelectedDocumentName = DocumentName;

        }



        public string SelectedUniversityName { get; set; }
        public string SelectedLastName { get; set; }
        public string SelectedEmail { get; set; }
        public string SelectedRegistrationNumber { get; set; }
        public string SelectedDocumentName { get; set; }



    }
}
