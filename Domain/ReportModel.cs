using Microsoft.AspNetCore.Mvc;
using PFStudio.PFSign.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFStudio.PFSign.Domain
{
    public class ReportModel
    {
        public DateTime? Begin { get; set; }
        public DateTime? End { get; set; }

        public IQueryable<Record> Filter(IQueryable<Record> query)
        {
            // 默认统计从昨天起往前7天的签到记录
            DateTime end = End?.ToUniversalTime() ?? DateTime.Today.AddDays(-1).ToUniversalTime();
            DateTime begin = Begin?.ToUniversalTime() ?? end.AddDays(-7);

            return from record in query
                   where record.SignInTime >= begin && record.SignInTime <= end
                   where record.SignOutTime != null
                   select record;
        }
    }
}
