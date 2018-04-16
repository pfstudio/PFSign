using PFStudio.PFSign.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFStudio.PFSign.Domain
{
    /// <summary>
    /// 查询Model
    /// </summary>
    public class QueryModel
    {
        public DateTime? Begin { get; set; }
        public DateTime? End { get; set; }
        public string StudentId { get; set; }
        public int Skip { get; set; } = 0;
        public int Size { get; set; } = 20;

        public IQueryable<Record> Filter(IQueryable<Record> query)
        {
            // 默认开始时间为当天
            DateTime beginTime = Begin?.ToUniversalTime() ?? DateTime.Today.ToUniversalTime();
            // 默认结束时间为开始时间后的一天
            DateTime endTime = End?.ToUniversalTime() ?? beginTime.AddDays(1);

            // 过滤query
            query = from record in query
                    where record.SignInTime >= beginTime && record.SignInTime <= endTime
                    select record;
            query = StudentId is null ? query : query.Where(r => r.StudentId == StudentId);

            // 以签到时间排序
            query = query.OrderBy(r => r.SignInTime);

            // 启用分页
            query = query.Skip(Skip).Take(Size);

            return query;
        }
    }
}
