using Microsoft.AspNetCore.Mvc.Rendering;
using Rectotarat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rectotarat.ViewModels
{
    public class EditRectorViewModel
    {
        public Rector CurrentRector { get; set; }
        public IEnumerable<Rector> Rectors { get; set; }
        public IEnumerable<University> Universities { get; set; }
        //Список университетов, не занятых ректорами
        public SelectList ListUniversities
        {
            get
            {
                List<int> uId = Universities.Select(i => i.UniversityId).ToList();
                List<int> urId = Rectors.Select(i => i.UniversityId).ToList();

                urId = uId.Except(urId).ToList<int>();
                int currentUniversityId = urId.DefaultIfEmpty(1).First();

                if (CurrentRector != null)
                {
                    urId = uId.Append(CurrentRector.UniversityId).Distinct().ToList<int>();
                    currentUniversityId = CurrentRector.UniversityId;
                }
                var universities = Universities.Where(i => urId.Any(t => t.Equals(i.UniversityId)));
                return new SelectList(universities, "UniversityId", "UniversityName", currentUniversityId);
            }
            set {; }
        }

    }
}
