using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rectotarat.Models
{
    // Типы значений показателя
    public enum IndicatorType
        {
        min, 
        max 
        }
    // Показатель для подведения итогов в заданном году
    public class Indicator
    {
        
        [Key]
        public int IndicatorId {get; set;}

        [Required]
        [Range(1, 100, ErrorMessage = "Недопустимое значение кода раздела")]        
        [Display(Name = "Код раздела")]        
        public byte IndicatorId1 {get; set;}

        [Display(Name = "Код подраздела")]       
        public byte? IndicatorId2 {get; set;}

        [Display(Name = "Код пункта")]
        public byte? IndicatorId3 {get; set;}

        [Display(Name = "Показатель")]
        public string IndicatorName {get; set;}

        [Display(Name = "Единица измерения")]
        public string IndicatorUnit {get; set;}

        [Display(Name = "Тип показателя")]
        [Required]
        public IndicatorType IndicatorType { get; set; }

        [Display(Name = "Описание показателя")]
        public string IndicatorDescription {get; set;}

        [Required]
        [Display(Name = "Год")]
        [Range(2010, 2050, ErrorMessage = "Недопустимый год")]
        public int Year {get; set;}

        [Display(Name = "Код показателя")]
        public string IndicatorCode
        {
            get
            {
                if (IndicatorId3 == null && IndicatorId2!=null)
                {
                    return Convert.ToString(IndicatorId1) + "." + Convert.ToString(IndicatorId2);
                }
                if (IndicatorId3 == null & IndicatorId2 == null)
                {
                    return Convert.ToString(IndicatorId1);
                }
                else
                {
                    return Convert.ToString(IndicatorId1) + "." + Convert.ToString(IndicatorId2) + "." + Convert.ToString(IndicatorId3);
                }
            }
        }

        public ICollection<Achievement> Achievements {get; set;}
        public Indicator()
        {
            this.IndicatorType = 0;
        }

    }
}
