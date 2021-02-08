using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rectotarat.Models
{
    // Университет
    public class University
    {
        [Key]
        [Display(Name = "Код")]
        public int UniversityId { get; set; }

        [Display(Name = "Университет")]
        public string UniversityName { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Display(Name = "Официальный сайт")]
        public string Website { get; set; }

        [Display(Name = "Логотип")]
        public string Logo { get; set; }

        public Rector Rector {get;set; }
        public ICollection<Achievement> Achievements {get; set;}
        public ICollection<News> Newss { get; set; }
    }
}
