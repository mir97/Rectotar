using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rectotarat.Models
{
    public class Raschet
    {
        public int Raschetid { get; set; }

        [Display(Name = "Университет")]
        public int UnivercityId { get; set; }
        public University Univercity { get; set; }

        [Display(Name = "Сумма мест")]
        public float SumaValue { get; set; }

        [Display(Name = "Место")]
        public float Position { get; set; }

        [Required]
        [Display(Name = "Год")]
        [Range(2010, 2050, ErrorMessage = "Недопустимый год")]
        public int Year { get; set; }

    }
}
