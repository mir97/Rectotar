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
using System.IO;
using System.Data;
using OfficeOpenXml;
using Microsoft.AspNetCore.Hosting;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Rectotarat.Controllers.Tabels
{
    public class RaschetController : Controller
    {
        private readonly RectoratContext _context;

        private IHostingEnvironment _hostingEnvironment;

        public RaschetController(RectoratContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index(int? currentYear, string UniversityName, string LastName, string Email, string RegistrationNumber, string DocumentName, int page = 1, SortState sortOrder = SortState.IndicatorCodeAsc, string cmd = "")
        {
            int pageSize = 20;   // количество элементов на странице
            int currYear = currentYear ?? DateTime.Now.Year;
            IEnumerable<Achievement> achievements = _context.Achievements
                    .Include(a => a.Indicator)
                    .Include(a => a.Univercity)
                    .Where(t => t.Year == currYear)
                    .OrderBy(s => s.Indicator.IndicatorCode);



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


            float[,] mest = new float[10,2];


            float[] sum = new float[100];
			
			/*Фильтруем записи по критерию  год тип показателя */
            achievements = achievements.Where(x => x.Year == currentYear && x.Indicator.IndicatorId2 != null && x.Indicator.IndicatorId3 == null);

			/*Групируем по id университета */
            var resul = from c in achievements
                        group c by c.UnivercityId;

            int i = 0;
            int n = 0;

			/*находим сумму баллов каждого униывера в отдельности*/
            foreach (IGrouping<int, Achievement> group in resul)
            {
                
                foreach (Achievement c in group)
                {
                    sum[i] = sum[i] + c.Position;
              
                }

				/* mest[i, 0]-сумма баллов mest[i, 1] - id университета*/
                mest[i, 0] = sum[i];
                mest[i, 1] = i;

                i++;
                n = i;
            }


            float temp = 0 ;
            float tempm = 0;
            float k = 0;

			/*Сортируем по местам по убыванию*/
            while (k != n+1)
            {

                for (i = 0; i < n-1; i++)
                {

                    if (mest[i, 0] > mest[i + 1, 0])
                    {
                        temp = mest[i, 0];
                        tempm = mest[i, 1];
                        mest[i, 0] = mest[i + 1, 0];
                        mest[i, 1] = mest[i + 1, 1];
                        mest[i + 1, 0] = temp;
                        mest[i + 1, 1] = tempm;
                    }

                }
                k++;
            }

            /*Добавляем полученные записиы*/
            if (_context.Raschets.Any())
            {

            }

            else
            {
                for (i = 0; i < n; i++)
                {
                    _context.Raschets.Add(new Raschet { Position = i + 1, UnivercityId = Convert.ToInt32(mest[i, 1]) + 1, SumaValue = mest[i, 0], Year = currYear });
                    _context.SaveChanges();
              
                }
      
            }

      
            IEnumerable<Raschet> ras = _context.Raschets.Include(a => a.Univercity).Where(x=>x.Year==currYear);  
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
                Raschets = ras.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                FilterViewModel = new FilterViewModel(UniversityName, LastName, Email, RegistrationNumber, DocumentName),
                ListYears = new SelectList(years.Distinct(), currYear)
            };
            return View(achievementsViewModel);
        }

       

        public async Task<IActionResult> OnPostExport(int? currentYear)
        {


            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"Отчетность.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            IEnumerable<Raschet> raschet = _context.Raschets.Include(a =>a.Univercity);
            

            var res1 = raschet.Where(c => c.Year == currentYear);

                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Demo");
                IRow row = excelSheet.CreateRow(0);

                row = excelSheet.CreateRow(1);
                row.CreateCell(0).SetCellValue("Отчетность за " + currentYear + " год");

                row = excelSheet.CreateRow(3);
                row.CreateCell(0).SetCellValue("№");
                row.CreateCell(1).SetCellValue("Университет");
                row.CreateCell(2).SetCellValue("Сумма мест");
                row.CreateCell(3).SetCellValue("Место");
                row.CreateCell(4).SetCellValue("Год");


                int i = 4;

                foreach (var item in res1)
                {
                    
                    row = excelSheet.CreateRow(i);

                    row.CreateCell(0).SetCellValue(Convert.ToString(i-3));
                    row.CreateCell(1).SetCellValue(Convert.ToString(item.Univercity.UniversityName));
                    row.CreateCell(2).SetCellValue(Convert.ToInt32(item.SumaValue));
                    row.CreateCell(3).SetCellValue(Convert.ToInt32(item.Position));
                    row.CreateCell(4).SetCellValue(Convert.ToInt32(item.Year));
                    i++;
                }

                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }


        public async Task<IActionResult> OnPostExportA(int? currentYear)
        {


            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"Результаты(Расчет).xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();

            var achievements = _context.Achievements
                    .Include(a => a.Indicator)
                    .Include(a => a.Univercity)
                    .Where(t => (t.Indicator.Year == currentYear))
                    .OrderBy(s => s.Indicator.IndicatorId1).OrderBy(s => s.Indicator.IndicatorId2);

            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Demo");
                IRow row = excelSheet.CreateRow(0);

                row = excelSheet.CreateRow(1);
                row.CreateCell(0).SetCellValue("Результаты(Расчет) за " + currentYear + " год");

                row = excelSheet.CreateRow(3);
                row.CreateCell(0).SetCellValue("Код показателя");
                row.CreateCell(1).SetCellValue("Показатель");
                row.CreateCell(6).SetCellValue("Университет");
                row.CreateCell(7).SetCellValue("Значение показателя");
                row.CreateCell(8).SetCellValue("Место");
                row.CreateCell(9).SetCellValue("Тип показателя");

                int i = 4;
                
                foreach (var item in achievements)
                {

                    row = excelSheet.CreateRow(i);

                    row.CreateCell(0).SetCellValue(Convert.ToString(item.Indicator.IndicatorCode));
                    row.CreateCell(1).SetCellValue(Convert.ToString(item.Indicator.IndicatorName));
                    row.CreateCell(6).SetCellValue(Convert.ToString(item.Univercity.UniversityName));
                    row.CreateCell(7).SetCellValue(Convert.ToString(item.IndicatorValue));
                    row.CreateCell(8).SetCellValue(Convert.ToString(item.Position));
                    row.CreateCell(9).SetCellValue(Convert.ToString(item.Indicator.IndicatorType));
                    i++;
                }

                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
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
