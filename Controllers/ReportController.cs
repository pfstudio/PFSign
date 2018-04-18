using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PFStudio.PFSign.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PFSign.Controllers
{
    [Route("/Report")]
    public class ReportController : Controller
    {
        // 签到记录上下文
        private readonly RecordDbContext _context;
        // 日志工具
        private readonly ILogger _logger;

        public ReportController(
            RecordDbContext context, ILogger<ReportController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<JsonResult> Summary()
        {
            // 统计从昨天起往前7天的签到记录
            DateTime end = DateTime.Today.AddDays(-1).ToUniversalTime();
            DateTime begin = end.AddDays(-7);

            var result = await (from record in _context.Records.AsNoTracking()
                                where record.SignInTime >= begin && record.SignInTime <= end
                                where record.SignOutTime != null
                                select record into r
                                group r by r.StudentId into g
                                select new
                                {
                                    StudentId = g.First().StudentId,
                                    Name = g.First().Name,
                                    TimeSpan = new TimeSpan(g.Sum(x => (x.SignOutTime.Value - x.SignInTime).Ticks))
                                }).ToListAsync();

            return Json(result);
        }

        [HttpGet("{studentId}")]
        public async Task<JsonResult> PersonalSummary([FromRoute]string studentId)
        {
            // 统计从昨天起往前7天的签到记录
            DateTime end = DateTime.Today.AddDays(-1).ToUniversalTime();
            DateTime begin = end.AddDays(-7);

            var result = await (from record in _context.Records.AsNoTracking()
                                where record.SignInTime >= begin && record.SignInTime <= end
                                where record.SignOutTime != null
                                where record.StudentId == studentId
                                select record into r
                                group r by r.SignInTime.Date into g
                                select new
                                {
                                    Date = g.Key.ToString("yyyy/MM/dd"),
                                    TimeSpan = new TimeSpan(g.Sum(x => (x.SignOutTime.Value - x.SignInTime).Ticks))
                                }).ToListAsync();
            return Json(result);
        }
    }
}
