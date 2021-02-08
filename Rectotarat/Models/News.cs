using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rectotarat.Models
{
    public class News
    {
        public int Newsid { get; set; }

        [Display(Name = "Университет")]
        public int UnivercityId { get; set; }
        public University Univercity { get; set; }

        [Display(Name = "Заголовок")]
        public string Header { get; set; }

        [Display(Name = "Сообщение")]
        public string Message { get; set; }

        [Display(Name = "Дата публикации")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yy}", ApplyFormatInEditMode = true)]
        public DateTime PublicDate { get; set; }

    }
}
