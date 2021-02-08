using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rectotarat.Models
{
        public class User : IdentityUser
        {

            [Display(Name = "Университет")]
            public int UniversityId { get; set; }
            [Display(Name = "Дата регистрации")]
            [DisplayFormat(DataFormatString = "{0:dd-MM-yy}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Date)]
            public DateTime RegistrationDate { get; set; }
            [Display(Name = "Университет")]
            public string UniversityName { get; set; }
            [Display(Name = "Роль")]
            public string RoleName { get; set; }

        }
}
