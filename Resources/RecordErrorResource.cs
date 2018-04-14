namespace PFStudio.PFSign.Resources
{
    public static class RecordErrorResource
    {
        /// <summary>
        /// 在签退时，所指向的Record错误或不存在
        /// </summary>
        public const string RecordError = "未签到或签到信息错误！";

        /// <summary>
        /// 数据库操作错误
        /// </summary>
        public const string DataBaseError = "数据库错误！";

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
