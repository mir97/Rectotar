using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rectotarat.Models;
using Microsoft.AspNetCore.Authorization;
using Rectotarat.ViewModels;

namespace Rectotarat.Controllers.Tabels
{
    [Authorize(Roles = "user, admin")]
    public class IndicatorsController : Controller
    {
        private readonly RectoratContext _context;

        public IndicatorsController(RectoratContext context)
        {
            _context = context;
        }

        // GET: Indicators
        public IActionResult Index(int? currentYear,bool? disableForEdition, int page = 1)
        {

            int pageSize = 20;
          

            bool enableForEdition = !(disableForEdition ?? true);
            int currYear = currentYear ?? DateTime.Now.Year;
            List<int> years = _context.Indicators
                .OrderByDescending(f => f.Year)
                .Select(f => f.Year)
                .ToList();
            years.Insert(0, currYear); years.Insert(0, currYear + 1);
            var ListYears = new SelectList(years.Distinct(), currYear);

            var achievementsCount = _context.Achievements.Where(m => m.Year == currYear).Count();

            IQueryable <Indicator> Indicatorsc = _context.Indicators.Where(t => t.Year == currYear).OrderBy(s => s.IndicatorId1).OrderBy(s => s.IndicatorId2).OrderBy(s => s.IndicatorId3);
       

            var count = Indicatorsc.Count();
            var items = Indicatorsc.Skip((page - 1) * pageSize).Take(pageSize);

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);


            IndicatorsViewModel indicators = new IndicatorsViewModel()
            {
                PageViewModel = pageViewModel,
                Indicators = items,
                ListYears = new SelectList(years.Distinct(), currYear),
                EnableForEdition = enableForEdition,
                AchievementsCount = achievementsCount
            };
            return View(indicators);
        }

        // POST: Indicators
        [HttpPost]
        public async Task<IActionResult> Index(int currentYear, bool? disableForEdition, string action)
        {
            bool enableForEdition = !(disableForEdition ?? true);

            List<int> years = _context.Indicators
                .OrderByDescending(f => f.Year)
                .Select(f => f.Year)
                .ToList();
            years.Insert(0, currentYear); years.Insert(0, currentYear + 1);
            var ListYears = new SelectList(years.Distinct(), currentYear);
            var indicators = _context.Indicators
                            .Where(t => t.Year == currentYear)
                            .OrderBy(s => s.IndicatorId1)
                            .OrderBy(s => s.IndicatorId2)
                            .OrderBy(s => s.IndicatorId3)
                            .ToList();
            var achievementsCount = _context.Achievements.Where(m => m.Year == currentYear).Count();

            switch (action)
            {
                case "FillDataFromLastYear":
                    if (indicators.Count() == 0)
                    {
                        var indicatorsLastYear = _context.Indicators
                            .Where(y => y.Year == (currentYear - 1));
                        foreach (var ind in indicatorsLastYear)
                        {
                            ind.Year = currentYear;
                            ind.IndicatorId = 0;
                            indicators.Add(ind);
                        }
                        _context.AddRange(indicators);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return View("Message", "Для текущего года уже загружены данные!");
                    }
                    break;
                case "FillDataForUniversities":
                    //Загрузка набора показателей дл¤ университетов на заданный год
                    string resultPublishIndicatorsForUniversities = await PublishIndicatorsForUniversities(currentYear);
                    if (resultPublishIndicatorsForUniversities == "")
                    {
                        return Redirect("~/Achievements/Index");
                    }
                    else
                    {
                        return View("Message", resultPublishIndicatorsForUniversities);
                    }
                default:
                    break;
            }
            IndicatorsViewModel indicatorsViewModel = new IndicatorsViewModel()
            {
                Indicators = indicators,
                ListYears = new SelectList(years.Distinct(), currentYear),
                EnableForEdition = enableForEdition,
                AchievementsCount = achievementsCount
            };
            return View(indicatorsViewModel);
        }


        // GET: Indicators/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var indicator = await _context.Indicators
                .SingleOrDefaultAsync(m => m.IndicatorId == id);
            if (indicator == null)
            {
                return NotFound();
            }
            return View(indicator);
        }

        // GET: Indicators/Create
        public IActionResult Create(int id)
        {
            ViewData["Year"] = id;
            if (User.IsInRole("user") && (DateTime.Now.Hour < 7 || DateTime.Now.Hour > 17))
            {
                return RedirectToAction("AccessDenied", "Account");
            }


            return View();
        }

        // POST: Indicators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IndicatorId,IndicatorId1,IndicatorId2,IndicatorId3,IndicatorName,IndicatorUnit,IndicatorType,IndicatorDescription,Year")] Indicator indicator)
        {
            if (User.IsInRole("user") && (DateTime.Now.Hour < 7 || DateTime.Now.Hour > 17))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (ModelState.IsValid)
            {
                _context.Add(indicator);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(indicator);
        }

        // GET: Indicators/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (User.IsInRole("user") && (DateTime.Now.Hour < 7 || DateTime.Now.Hour > 17))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var indicator = await _context.Indicators.SingleOrDefaultAsync(m => m.IndicatorId == id);
            if (indicator == null)
            {
                return NotFound();
            }
            return View(indicator);
        }

        // POST: Indicators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("IndicatorId,IndicatorId1,IndicatorId2,IndicatorId3,IndicatorName,IndicatorUnit,IndicatorType,IndicatorDescription,Year")] Indicator indicator)
        {

            if (User.IsInRole("user") && (DateTime.Now.Hour < 7 || DateTime.Now.Hour > 17))
            {
                return RedirectToAction("AccessDenied", "Account");
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(indicator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IndicatorExists(indicator.IndicatorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { currentYear = indicator.Year, disableForEdition = false });
            }
            return View(indicator);
        }

        // GET: Indicators/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            if (User.IsInRole("user") && (DateTime.Now.Hour < 7 || DateTime.Now.Hour > 17))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }
            var indicator = await _context.Indicators
                .SingleOrDefaultAsync(m => m.IndicatorId == id);
            if (indicator == null)
            {
                return NotFound();
            }
            return View(indicator);
        }

        // POST: Indicators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int IndicatorId)
        {
            if (User.IsInRole("user") && (DateTime.Now.Hour < 7 || DateTime.Now.Hour > 17))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var indicator = await _context.Indicators.SingleOrDefaultAsync(m => m.IndicatorId == IndicatorId);
            _context.Indicators.Remove(indicator);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { currentYear = indicator.Year, disableForEdition = false });
        }

        private bool IndicatorExists(int id)
        {
            return _context.Indicators.Any(e => e.IndicatorId == id);
        }

        // Загрузка показателей дл¤ университетов за заданный год
        private async Task<string> PublishIndicatorsForUniversities(int currYear)
        {
            // Удаление данных университетов за заданный год
            var achievements = _context.Achievements.Where(m => m.Year == currYear);
            if (achievements.Count() > 0)
            {
                _context.Achievements.RemoveRange(achievements);
                await _context.SaveChangesAsync();
            };

            // Вставка данных университетов за заданный год
            string publishResult = "Невозможно вставить данные";
            List<int> indicators = _context.Indicators.Where(y => y.Year == currYear).Select(id => id.IndicatorId).ToList();
            List<int> universities = _context.Universitys.Select(u => u.UniversityId).ToList();
            try
            {
                foreach (int university in universities)
                {
                    foreach (int indicator in indicators)
                    {
                        Achievement achievement = new Achievement
                        {
                            Year = currYear,
                            IndicatorId = indicator,
                            UnivercityId = university
                        };
                        _context.Add(achievement);
                    }
                }
                await _context.SaveChangesAsync();
                publishResult = "";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                publishResult = ex.Message;
                return publishResult;
            }
            return publishResult;
        }
    }
}
