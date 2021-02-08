using System.ComponentModel.DataAnnotations;

namespace Rectotarat.Models
{
    // Фактическое достижение университета по заданному показателю в заданном году
    public class Achievement
    {
        [Key]
        public int AchievementId {get; set;}

        [Display(Name = "Университет")]
        public int UnivercityId { get; set; }
        public University Univercity {get; set;}

        [Display(Name = "Показатель")]
        public int IndicatorId { get; set; }
        public Indicator Indicator {get; set;}

        [Display(Name = "Значение показателя")]
        public float IndicatorValue { get; set; }

        [Display(Name = "Место")]
        public float Position { get; set; }

        [Required]
        [Display(Name = "Год")]
        [Range(2010, 2050, ErrorMessage = "Недопустимый год")]
        public int Year { get; set; }

    }
}
