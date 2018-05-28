using Microsoft.EntityFrameworkCore;
using PFSign.Data;
using PFSign.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using PFSign.Extensions;

namespace PFSign.Repositorys
{
    public class EFReportRepository : IReportRepository
    {
        private readonly RecordDbContext _dbContext;

        public EFReportRepository(RecordDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<RecordDuration>> ReportAllAsync()
        {
            // 默认报告时间段为本周
            DateTime
                begin = DateTime.Today.StartOfWeek(),
                end = DateTime.Today.EndOfWeek();
            // 统计签到记录
            var result = await (from r in _dbContext.Records.AsNoTracking()
                                where r.SignOutTime != null
                                where r.SignInTime >= begin
                                && r.SignInTime < end
                                group r by new { r.StudentId, r.Name } into g
                                select new RecordDuration
                                {
                                    StudentId = g.Key.StudentId,
                                    Name = g.Key.Name,
                                    Duration = new TimeSpan(g.Sum(r => r.GetDuration().Ticks))
                                }).ToListAsync();
            return result;
        }

        public async Task<List<DateDuration>> ReportWithAsync(string studentId)
        {
            // 默认报告时间段为本周
            DateTime
                begin = DateTime.Today.StartOfWeek(),
                end = DateTime.Today.EndOfWeek();
            // 统计签到记录
            var result = await (from r in _dbContext.Records.AsNoTracking()
                                where r.StudentId == studentId
                                where r.SignOutTime != null
                                where r.SignInTime >= begin
                                && r.SignInTime < end
                                group r by r.SignInTime.Date into g
                                select new DateDuration
                                {
                                    Date = g.Key,
                                    Duration = new TimeSpan(g.Sum(r => r.GetDuration().Ticks))
                                }).ToListAsync();

            return result;
        }
    }
}
