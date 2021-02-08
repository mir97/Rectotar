using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rectotarat.Models;
using Rectotarat.ViewModels;
using GomelRectorCouncil.Calculations;
using Microsoft.AspNetCore.Authorization;

namespace Rectotarat.Controllers.Tabels
{
    [Authorize(Roles = "admin")]
    public class AchievementsResultController : Controller
    {
        private readonly RectoratContext _context;

        public AchievementsResultController(RectoratContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? currentYear, string UniversityName, string LastName, string Email, string RegistrationNumber, string DocumentName, int page = 1, string cmd = "")
        {
            int pageSize = 20;   // количество элементов на странице
            int currYear = currentYear ?? DateTime.Now.Year;
            IEnumerable<Achievement> achievements = _context.Achievements
                    .Include(a => a.Indicator)
                    .Include(a => a.Univercity)
                    .Where(t => t.Year == currYear)
                    .OrderBy(s => s.Indicator.IndicatorId1).OrderBy(s => s.Indicator.IndicatorId2);



            if (UniversityName != null)
            {
                achievements = achievements.Where(x => x.Univercity.UniversityName.ToString().Contains(UniversityName.ToString()));
            }


            //Вычисление занятых мест
            if (cmd == "CalculatePositions")
            {
                achievements = Positions.Get(achievements.ToList());
                _context.UpdateRange(achievements);
                _context.SaveChanges();
            }



            /*
            // сортировка
            switch (sortOrder)
            {
                case SortState.IndicatorCodeDesc:
                    achievements = achievements.OrderByDescending(s => s.Indicator.IndicatorCode);
                    break;
                case SortState.UniversityNameAsc:
                    achievements = achievements.OrderBy(s => s.Univercity.UniversityName);
                    break;
                case SortState.UniversityNameDesc:
                    achievements = achievements.OrderByDescending(s => s.Univercity.UniversityName);
                    break;
                default:
                    achievements = achievements.OrderBy(s => s.Indicator.IndicatorCode);
                    break;
            }

    */



            int count = achievements.Count();
            List<int> years = _context.Indicators
                .OrderByDescending(f => f.Year)
                .Select(f => f.Year)
                .ToList();
            years.Insert(0, currYear); years.Insert(0, currYear + 1);

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            IndexViewModel achievementsViewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Achievements = achievements.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                FilterViewModel = new FilterViewModel(UniversityName, LastName, Email, RegistrationNumber, DocumentName),
                ListYears = new SelectList(years.Distinct(), currYear)
            };
            return View(achievementsViewModel);
        }


        // GET: Achievements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var achievement = await _context.Achievements
                .Include(a => a.Indicator)
                .Include(a => a.Univercity)
                .SingleOrDefaultAsync(m => m.AchievementId == id);
            if (achievement == null)
            {
                return NotFound();
            }
            return View(achievement);
        }

    }
}
