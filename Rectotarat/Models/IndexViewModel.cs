using Microsoft.AspNetCore.Mvc.Rendering;
using Rectotarat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rectotarat.Models
{
    public class IndexViewModel
    {

        public IEnumerable<Indicator> Indicators { get; set; }
        public IEnumerable<University> Universitys { get; set; }
        public IEnumerable<Achievement> Achievements { get; set; }
        public IEnumerable<Document> Documents { get; set; }
        public IEnumerable<Chairperson> Chairpersons { get; set; }
        public IEnumerable<Rector> Rectors { get; set; }
        public IEnumerable<Raschet> Raschets { get; set; }
        public IEnumerable<User> users { get; set; }
        public IEnumerable<News> Newss { get; set; }

        public PageViewModel PageViewModel { get; set; }

        public FilterViewModel FilterViewModel { get; set; }

        public SortViewModel SortViewModel { get; set; }

        public SelectList ListYears { get; set; }

        public SelectList Listemail { get; set; }


    }
}
