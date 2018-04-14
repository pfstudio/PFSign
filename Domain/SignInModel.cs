using Microsoft.EntityFrameworkCore;
using PFStudio.PFSign.Models;
using PFStudio.PFSign.Resources;
using System.Linq;
using System.Threading.Tasks;

namespace PFStudio.PFSign.Domain
{
    /// <summary>
    /// 签到Model
    /// </summary>
    public class SignInModel
    {
        // 学号
        public string StudentId { get; set; }
        // 姓名
        public string Name { get; set; }

        public Record Create() => new Record(StudentId, Name);

        /// <summary>
        /// 检查是否允许签到
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public async Task<RecordResult> Check(IQueryable<Record> records)
        {
            // 检查签到参数
            if (!CheckInfo(out string infoError))
            {
                return RecordResult.Fail(infoError);
            }

            // 检测签到状态
            if (await (from r in records
                       where r.StudentId == StudentId
                       && r.SignOutTime == null
                       select r).CountAsync() > 0)
            {
                return RecordResult.Fail(RecordErrorResource.StudentSigned);
            }

            return RecordResult.Success();
        }

        /// <summary>
        /// 检查签到参数，并返回相应错误信息。
        /// </summary>
        /// <param name="studentId">学号</param>
        /// <param name="name">姓名</param>
        /// <param name="error">错误信息</param>
        /// <returns>检测结果</returns>
        private bool CheckInfo(out string error)
        {
            if (string.IsNullOrEmpty(StudentId) || string.IsNullOrEmpty(Name))
            {
                error = RecordErrorResource.UserInfoIncorrect;
                return false;
            }

            error = null;
            return true;
        }
    }
}
