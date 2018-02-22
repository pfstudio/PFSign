using System;

namespace PFSignDemo.Models
{
    /// <summary>
    /// 签到记录
    /// </summary>
    public class Record
    {
        public Guid Id { get; set; }
        // 学号
        public string StudentId { get; set; }
        // 姓名
        public string Name { get; set; }
        // 签到时间
        public DateTime SignInTime { get; set; }
        // 签退时间
        public DateTime? SignOutTime { get; set; }
        // 座位号
        public int Seat { get; set; }

        /// <summary>
        /// 创建签到记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Record Create(SignInModel model)
        {
            return new Record()
            {
                Id = Guid.NewGuid(),
                StudentId = model.StudentId,
                Name = model.Name,
                SignInTime = DateTime.UtcNow,
                Seat = model.Seat.Value
            };
        }
    }
}
