using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rectotarat.Models
{
    // Председатель совета ректоров
    public class Chairperson
    {
        [Key]
        public int ChairpersonId { get; set; }
        [Display(Name="Дата назначения")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Display(Name="Дата отставки")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? StopDate { get; set; }
        [Display(Name = "ФИО")]
        public int RectorId { get; set; }
        public Rector Rector { get; set; }
        public ICollection<Document> Documents { get; set; }

    }
}
