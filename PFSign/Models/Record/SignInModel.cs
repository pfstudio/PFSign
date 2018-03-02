using Microsoft.EntityFrameworkCore;
using PFStudio.PFSign.Resources;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PFStudio.PFSign.Models
{
    /// <summary>
    /// 签到Model
    /// </summary>
    public class SignInModel
    {
        // Claim Type
        private readonly static string StudentIdType = "extension_StudentId";
        private readonly static string NameType = "name";

        // 学号
        public string StudentId { get; set; }
        // 姓名
        public string Name { get; set; }
        // 座位号
        public int? Seat { get; set; }

        /// <summary>
        /// 创建签到Model
        /// </summary>
        /// <param name="user">Controller 中的User</param>
        /// <param name="seat">座位号</param>
        /// <returns></returns>
        public static SignInModel Create(ClaimsPrincipal user, int? seat)
        {
            return new SignInModel()
            {
                StudentId = user.FindFirstValue(StudentIdType),
                Name = user.FindFirstValue(NameType),
                Seat = seat
            };
        }

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

            // 检测当前座位状态
            if (await (from r in records
                       where r.Seat == Seat
                       && r.SignOutTime == null
                       select r).CountAsync() > 0)
            {
                return RecordResult.Fail(RecordErrorResource.SeatSigned);
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
        /// <param name="seat">座位号</param>
        /// <param name="error">错误信息</param>
        /// <returns>检测结果</returns>
        private bool CheckInfo(out string error)
        {
            if (StudentId == null || Name == null)
            {
                error = RecordErrorResource.UserInfoIncorrect;
                return false;
            }

            if (Seat == null)
            {
                error = RecordErrorResource.SeatIncorrect;
                return false;
            }

            error = null;
            return true;
        }
    }
}
