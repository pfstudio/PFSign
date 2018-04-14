using System;
using System.ComponentModel.DataAnnotations;

namespace PFStudio.PFSign.Models
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

        public Record() { }

        public Record(string studentId, string name)
        {
            StudentId = studentId;
            Name = name;
        }

        public void SignIn()
        {
            SignInTime = DateTime.UtcNow;
        }

        public void SignOut()
        {
            SignOutTime = DateTime.UtcNow;
        }

        public void SignOutWithTimeOut()
        {
            SignOutTime = SignInTime.AddHours(8);
        }
    }
}
