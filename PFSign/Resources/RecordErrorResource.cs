using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFStudio.PFSign.Resources
{
    public static class RecordErrorResource
    {
        /// <summary>
        /// 在签退时，所指向的Record错误或不存在
        /// </summary>
        public const string RecordError = "签到信息错误！";

        /// <summary>
        /// Seat在签到时的参数错误
        /// </summary>
        public const string SeatIncorrect = "座位信息错误！";

        /// <summary>
        /// Seat在签到时的状态错误
        /// </summary>
        public const string SeatSigned = "当前座位已有人！";

        /// <summary>
        /// StudentId在签退时的状态错误
        /// </summary>
        public const string StudentSigned = "上次签到未签退！";

        /// <summary>
        /// StudentId和Name在签到时的参数错误
        /// </summary>
        public const string UserInfoIncorrect = "用户信息错误！";
    }
}
