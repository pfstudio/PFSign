using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PFSign.Repositorys;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PFSign.Controllers
{
    [Route("/api/Report")]
    public class ReportController : Controller
    {
        // 报告仓储
        private readonly IReportRepository  _reportRepository;
        // 日志工具
        private readonly ILogger _logger;
        // 默认要求时间
        private readonly TimeSpan requiredHours = new TimeSpan(8, 0, 0);

        public ReportController(
            IReportRepository reportRepository, ILogger<ReportController> logger)
        {
            _reportRepository = reportRepository;
            _logger = logger;
        }

        /// <summary>
        /// 所有人的签到时间Report
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> Summary()
        {
            var recordDurations = await _reportRepository.ReportAllAsync();
            var result = recordDurations.Select(r => new
            {
                r.StudentId,
                r.Name,
                r.Duration,
                Required = requiredHours
            });

            return Json(result);
        }

        /// <summary>
        /// 个人的签到时间Report
        /// </summary>
        /// <param name="model"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet("{studentId}")]
        public async Task<IActionResult> PersonalSummary([FromRoute]string studentId)
        {
            if (studentId == null)
            {
                return NotFound();
            }

            var dateDurations = await _reportRepository.ReportWithAsync(studentId);

            return Json(new
            {
                studentId,
                Total = new TimeSpan(dateDurations.Sum(x => x.Duration.Ticks)),
                Durations = dateDurations
            });
        }
    }
}
