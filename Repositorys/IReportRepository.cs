using PFSign.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PFSign.Repositorys
{
    public interface IReportRepository
    {
        /// <summary>
        /// 生成所有人的签到报告
        /// </summary>
        Task<List<RecordDuration>> ReportAllAsync();
        /// <summary>
        /// 查询指定学号某人的签到报告
        /// </summary>
        /// <param name="studentId">学号</param>
        Task<List<DateDuration>> ReportWithAsync(string studentId);
    }
}
