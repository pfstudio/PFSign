using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PFStudio.PFSign.Data;
using PFStudio.PFSign.Domain;
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
        // 默认要求时间
        private readonly TimeSpan requiredHours = new TimeSpan(8, 0, 0);

        public ReportController(
            RecordDbContext context, ILogger<ReportController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// 所有人的签到时间Report
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> Summary(ReportModel model)
        {
            var result = await (from record in model.Filter(_context.Records.AsNoTracking())
                                group record by record.StudentId into g
                                select new
                                {
                                    StudentId = g.First().StudentId,
                                    Name      = g.First().Name,
                                    TimeSpan  = new TimeSpan(g.Sum(record => record.GetDuration().Ticks))
                                }).ToListAsync();

            return Json(result);
        }

        /// <summary>
        /// 个人的签到时间Report
        /// </summary>
        /// <param name="model"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet("{studentId}")]
        public async Task<JsonResult> PersonalSummary(ReportModel model, [FromRoute]string studentId)
        {
            var result = await (from record in model.Filter(_context.Records.AsNoTracking())
                                where record.StudentId == studentId
                                select record).ToListAsync();

            // 当无记录时，默认返回姓名为None
            // TODO: 计划引入每个人的信息表
            return Json(new
            {
                StudentId = studentId,
                Name      = result.FirstOrDefault()?.Name ?? "None",
                TimeSpan  = new TimeSpan(result.Sum(record => record.GetDuration().Ticks)),
                Required  = requiredHours
            });
        }

        /// <summary>
        /// 个人的签到详情Report
        /// </summary>
        /// <param name="model"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet("{studentId}/detail")]
        public async Task<JsonResult> PersonalDetail(ReportModel model, [FromRoute]string studentId)
        {
            var result = await (from record in model.Filter(_context.Records.AsNoTracking())
                                where record.StudentId == studentId
                                select record into r
                                group r by r.SignInTime.Date into g
                                select new
                                {
                                    Date     = g.Key.ToString("yyyy/MM/dd"),
                                    TimeSpan = new TimeSpan(g.Sum(record => record.GetDuration().Ticks))
                                }).ToListAsync();

            return Json(result);
        }
    }
}
