using System;
using System.ComponentModel.DataAnnotations;

namespace PFSign.Models
{
    /// <summary>
    /// 签到记录
    /// </summary>
    public class Record
    {
        [Key]
        public int Id { get; set; }
        // 学号
        public string StudentId { get; private set; }
        // 姓名
        public string Name { get; private set; }
        // 签到时间
        public DateTime SignInTime { get; private set; }
        // 签退时间
        public DateTime? SignOutTime { get; private set; }
        // 超时时间
        private const int TimeOutHours = 8;

        // 默认构造
        private Record() { }

        // 签到
        public static Record SignIn(string studentId, string name)
        {
            return new Record
            {
                StudentId = studentId,
                Name = name,
                SignInTime = DateTime.UtcNow
            };
        }

        // 签退
        public void SignOut()
        {
            SignOutTime = this.IsTimeOut() ?
                SignInTime.AddHours(TimeOutHours) : DateTime.UtcNow;
        }

        // 计算签到时长
        public TimeSpan GetDuration()
        {
            return (SignOutTime ?? SignInTime) - SignInTime;
        }

        // 判断超时
        public bool IsTimeOut()
        {
            return SignInTime.AddHours(TimeOutHours) < DateTime.UtcNow;
        }
    }
}
