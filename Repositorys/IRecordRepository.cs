using PFSign.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PFSign.Repositorys
{
    public interface IRecordRepository
    {
        /// <summary>
        /// 查找签到记录
        /// </summary>
        /// <param name="begin">起始日期</param>
        /// <param name="end">结束日期</param>
        /// <param name="studentId">学号</param>
        /// <param name="skip">跳过条数</param>
        /// <param name="size">返回条数</param>
        /// <returns>签到记录的列表</returns>
        Task<List<Record>> QueryAsync(DateTime begin, DateTime end, string studentId, int skip, int size);
        /// <summary>
        /// 清空所有未签退的人员
        /// </summary>
        Task ClearAllUnSignOutAsync();
        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="studentId">学号</param>
        /// <param name="name">姓名</param>
        Task SignInAsync(string studentId, string name);
        /// <summary>
        /// 签退
        /// </summary>
        /// <param name="studentId">学号</param>
        Task SignOutAsync(string studentId);
        /// <summary>
        /// 判断当前的签到状态
        /// </summary>
        /// <param name="studentId">学号</param>
        bool IsSigned(string studentId);
    }
}
