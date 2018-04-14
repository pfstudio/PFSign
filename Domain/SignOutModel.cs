using Microsoft.EntityFrameworkCore;
using PFStudio.PFSign.Models;
using PFStudio.PFSign.Resources;
using System.Linq;
using System.Threading.Tasks;

namespace PFStudio.PFSign.Domain
{
    /// <summary>
    /// 签退Model
    /// </summary>
    public class SignOutModel
    {
        // 学号
        public string StudentId { get; set; }

        /// <summary>
        /// 检查是否可以签退
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public async Task<RecordResult> Check(IQueryable<Record> records)
        {
            // 检查参数
            if (string.IsNullOrEmpty(StudentId))
            {
                return RecordResult.Fail(RecordErrorResource.UserInfoIncorrect);
            }

            // 检测签到状态
            if (await (from r in records
                       where r.SignOutTime == null
                       && r.StudentId == StudentId
                       select r).CountAsync() == 0)
            {
                return RecordResult.Fail(RecordErrorResource.RecordError);
            }

            return RecordResult.Success();
        }
    }
}
