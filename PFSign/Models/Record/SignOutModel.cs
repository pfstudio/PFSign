﻿using Microsoft.EntityFrameworkCore;
using PFStudio.PFSign.Resources;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PFStudio.PFSign.Models
{
    /// <summary>
    /// 签退Model
    /// </summary>
    public class SignOutModel
    {
        private readonly static string StudentIdType = "extension_StudentId";

        // 学号
        public string StudentId { get; set; }

        /// <summary>
        /// 创建签退Model
        /// </summary>
        /// <param name="user">Controller 中的User</param>
        /// <returns></returns>
        public static SignOutModel Create(ClaimsPrincipal user)
        {
            return new SignOutModel()
            {
                StudentId = user.FindFirstValue(StudentIdType)
            };
        }

        /// <summary>
        /// 检查是否可以签退
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public async Task<RecordResult> Check(IQueryable<Record> records)
        {
            // 检查参数
            if (StudentId == null)
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
