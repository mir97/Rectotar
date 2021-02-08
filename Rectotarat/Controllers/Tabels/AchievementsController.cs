using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rectotarat.Models;
using Rectotarat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace Rectotarat.Controllers.Tabels
{
    [Authorize(Roles = "user,admin")]
    public class AchievementsController : Controller
    {
        private readonly RectoratContext _context;
        private readonly RectoratContext _contextUser;

        public AchievementsController(RectoratContext context, RectoratContext contextUser)
        {
            _context = context;
            _contextUser = contextUser;
        }

        // GET: Achievements
        public IActionResult Index(int? currentYear, int page = 1, SortState sortOrder = SortState.IndicatorCodeAsc)
        {

            int pageSize = 20;

            int currYear = currentYear ?? DateTime.Now.Year;

            
            int univercityId = GetUniversiryId();

         

            string[] name = _context.Universitys.Where(t => t.UniversityId == univercityId).Select(t => t.UniversityName).ToArray<string>();
            string[] nameuser = _contextUser.Users.Where(t => t.UserName == User.Identity.Name).Select(t => t.UserName).ToArray<string>();



            var achievements = _context.Achievements
                    .Include(a => a.Indicator)
                    .Include(a => a.Univercity)
                    .Where(t => (t.Indicator.Year == currYear) & (t.UnivercityId == univercityId))
                    .OrderBy(s => s.Indicator.IndicatorId1).OrderBy(s => s.Indicator.IndicatorId2);

            /*
            switch (sortOrder)
            {
                case SortState.IndicatorCodeDesc:
                    achievements = achievements.OrderByDescending(s => s.Indicator.IndicatorCode);
                    break;
                default:
                    achievements = achievements.OrderBy(s => s.Indicator.IndicatorCode);
                    break;
            }
            */


            TempData["Message"] = name[0];
            TempData["Message1"] = nameuser[0];

           
            

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
                SortViewModel = new SortViewModel(sortOrder),
                Achievements = achievements.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                ListYears = new SelectList(years.Distinct(), currYear),
            };
            return View(achievementsViewModel);
        }

        static List<Achievement> books = new List<Achievement>();

        public string GetAchievements(int? currentYear, string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            int currYear = currentYear ?? DateTime.Now.Year;
            int univercityId = GetUniversiryId();
            if (univercityId == 0)
            {
                string message = "Текущий пользователь не привязан к университету";
                return message;
            }
            sord = (sord == null) ? "" : sord;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var achievements = _context.Achievements.Where(t => (t.Year == currYear) & (t.UnivercityId == univercityId)).Select(
                    t => new
                    {
                        t.AchievementId,
                        t.Indicator.IndicatorCode,
                        t.Indicator.IndicatorName,
                        t.IndicatorValue
                    });

            int totalRecords = achievements.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sord.ToUpper() == "DESC")
            {
                achievements = achievements.OrderByDescending(t => t.IndicatorCode);
                achievements = achievements.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                achievements = achievements.OrderBy(t => t.IndicatorCode);
                achievements = achievements.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = achievements
            };
            return JsonConvert.SerializeObject(jsonData);
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

        // GET: Achievements/Create
        public IActionResult Create()
        {
            ViewData["IndicatorId"] = new SelectList(_context.Indicators, "IndicatorId", "IndicatorName");
            ViewData["UnivercityId"] = new SelectList(_context.Universitys, "UniversityId", "UniversityName");
            return View();
        }

        // POST: Achievements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AchievementId,UnivercityId,IndicatorId,IndicatorValue,Position,Year")] Achievement achievement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(achievement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IndicatorId"] = new SelectList(_context.Indicators, "IndicatorId", "IndicatorName");
            ViewData["UnivercityId"] = new SelectList(_context.Universitys, "UniversityId", "UniversityName");
            return View(achievement);
        }

        // GET: Achievements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var achievement = await _context.Achievements.SingleOrDefaultAsync(m => m.AchievementId == id);
            if (achievement == null)
            {
                return NotFound();
            }
            ViewData["IndicatorId"] = new SelectList(_context.Indicators, "IndicatorId", "IndicatorName", achievement.IndicatorId);
            ViewData["UnivercityId"] = new SelectList(_context.Universitys, "UniversityId", "UniversityName", achievement.UnivercityId);
            return View(achievement);
        }

        // POST: Achievements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AchievementId,UnivercityId,IndicatorId,IndicatorValue,Position,Year")] Achievement achievement)
        {
            if (id != achievement.AchievementId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try

                {
                    _context.Update(achievement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AchievementExists(achievement.AchievementId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IndicatorId"] = new SelectList(_context.Indicators, "IndicatorId", "IndicatorName", achievement.IndicatorId);
            ViewData["UnivercityId"] = new SelectList(_context.Universitys, "UniversityId", "UniversityName", achievement.UnivercityId);
            return View(achievement);
        }

        // GET: Achievements/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Achievements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var achievement = await _context.Achievements.SingleOrDefaultAsync(m => m.AchievementId == id);
            _context.Achievements.Remove(achievement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AchievementExists(int id)
        {
            return _context.Achievements.Any(e => e.AchievementId == id);
        }
        
        private int GetUniversiryId()
        {
            int[] universiryId = _contextUser.Users.Where(t => t.UserName == User.Identity.Name).Select(t => t.UniversityId).ToArray<int>();
            return universiryId[0];
        }
    }
}
