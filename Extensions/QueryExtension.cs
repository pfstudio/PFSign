using PFSign.Domain;
using PFSign.Models;
using System;
using System.Linq;

namespace PFSign.Extensions
{
    /// <summary>
    /// 封装对IQueryable对象的筛选
    /// </summary>
    public static class QueryExtension
    {
        public static IQueryable<Record> Filter(this IQueryable<Record> query, QueryModel model)
        {
            // 默认开始时间为当天
            DateTime begin = model.Begin?.ToUniversalTime() ?? DateTime.Today.ToUniversalTime();
            // 默认结束时间为开始时间后的一天
            DateTime end = model.End?.ToUniversalTime() ?? begin.AddDays(1);

            // 过滤query
            query = query.Where(r => r.SignInTime >= begin && r.SignInTime <= end);
            query = model.StudentId is null ? query : query.Where(r => r.StudentId == model.StudentId);

            // 以签到时间排序
            query = query.OrderBy(r => r.SignInTime);

            // 启用分页
            query = query.Skip(model.Skip).Take(model.Size);

            return query;
        }

        public static IQueryable<Record> Filter(this IQueryable<Record> query, ReportModel model)
        {
            DateTime begin, end;
            // 默认统计本周的签到记录
            if (model.Begin != null && model.End != null)
            {
                begin = model.Begin.Value;
                end = model.End.Value;
            }
            else
            {
                begin = DateTime.Today.StartOfWeek();
                end = DateTime.Today.EndOfWeek();
            }

            return from record in query
                   where record.SignInTime >= begin && record.SignInTime <= end
                   where record.SignOutTime != null
                   select record;
        }
    }
}
