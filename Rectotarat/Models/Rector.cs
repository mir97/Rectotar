using System.ComponentModel.DataAnnotations;

namespace Rectotarat.Models
{
    // Ректор университета
    public class Rector
    {
        [Key]        
        public int RectorId { get; set; }

        [StringLength(50)]
        [Required (ErrorMessage = "Не указана фамилия")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Имя не может быть длиннее чем 50 символов.")]
        [Display(Name = "Имя")]
        public string FirstMidName { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Отчество не может быть длиннее чем 60 символов.")]
        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Display(Name = "ФИО ректора")]
        public string FullName
        {
            get
            {
                return LastName + " " + FirstMidName+ " " + MiddleName;
            }
        }
        
        [EmailAddress (ErrorMessage = "Некорректный адрес")]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

        [Display(Name = "Фото")]
        public string Photo {get; set;}

        public int UniversityId {get; set;}

        public University University { get; set; }
    }
}
