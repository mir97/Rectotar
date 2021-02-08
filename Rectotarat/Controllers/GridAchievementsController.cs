using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Rectotarat.Data;
using Rectotarat.Models;
using Newtonsoft.Json;

namespace Rectotarat.Controllers
{
    [Authorize(Roles = "user, admin")]
    public class GridAchievementsController : Controller
    {

        private readonly RectoratContext _context;
        private readonly RectoratContext _contextUser;


        public GridAchievementsController(RectoratContext context, RectoratContext contextUser)
        {
            _context = context;
            _contextUser = contextUser;
        }

        // GET: Achievements
        public IActionResult Index(int? currentYear)
        {
            int univercityId = GetUniversiryId();
            int currYear = currentYear ?? DateTime.Now.Year;
            string[] name = _context.Universitys.Where(t => t.UniversityId == univercityId).Select(t => t.UniversityName).ToArray<string>();
            string[] nameuser = _contextUser.Users.Where(t => t.UserName == User.Identity.Name).Select(t => t.UserName).ToArray<string>();
            List<int> years = _context.Indicators
                .OrderByDescending(f => f.Year)
                .Select(f => f.Year)
                .ToList();
            years.Insert(0, currYear); years.Insert(0, currYear + 1);
            SelectList ListYears = new SelectList(years.Distinct(), currYear);

            TempData["Message"] = name[0];
            TempData["Message1"] = nameuser[0];

            return View(ListYears);
        }

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
            var achievements = _context.Achievements.Where(t => (t.Year == currYear) & (t.UnivercityId == univercityId)).OrderBy(s => s.Indicator.IndicatorId1).OrderBy(s => s.Indicator.IndicatorId2).OrderBy(s => s.Indicator.IndicatorId3).Select(
                    t => new
                    {
                        t.AchievementId,
                        t.Indicator.IndicatorCode,
                        t.Indicator.IndicatorName,
                        t.IndicatorValue,
                        t.Position,
                        t.UnivercityId,
                        t.IndicatorId
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


        // GET: Achievements/Edit/5
        public string Edit(Achievement achievement)
        {
            string msg = "Модель не прошла валидацию";

            if (ModelState.IsValid)
            {
                try
                {
                    achievement.UnivercityId = GetUniversiryId();
                    msg = "Сохранено";
                    _context.Update(achievement);
                    _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AchievementExists(achievement.AchievementId))
                    {
                        msg = "Не найдено";
                        return msg;
                    }
                    else
                    {
                        throw;
                    }
                }
                return msg;
            }
            return msg;
        }



        private bool AchievementExists(int id)
        {
            return _context.Achievements.Any(e => e.AchievementId == id);
        }
        //Получение кода университета, связанного с пользователем
        private int GetUniversiryId()
        {
            int[] universiryId = _contextUser.Users.Where(t => t.UserName == User.Identity.Name).Select(t => t.UniversityId).ToArray<int>();
            return universiryId[0];
        }
    }
}
