namespace PFSign.Resources
{
    /// <summary>
    /// 有关签到记录的一些错误信息
    /// </summary>
    public static class RecordErrorResource
    {
        /// <summary>
        /// 签到记录错误
        /// </summary>
        public const string RecordError = "签到信息错误或不存在！";

        /// <summary>
        /// 数据库操作错误
        /// </summary>
        public const string DataBaseError = "数据库错误！";

        /// <summary>
        /// 在签到时为已签到状态
        /// </summary>
        public const string HasSigned = "上次签到未签退！";

        /// <summary>
        /// 在签退时为未签到状态
        /// </summary>
        public const string NotSigned = "没有找到对应的签到记录！";

        /// <summary>
        /// StudentId参数错误
        /// </summary>
        public const string StudentIdIncorrect = "学号格式不符合要求！";

        /// <summary>
        /// Name参数错误
        /// </summary>
        public const string NameIncorrect = "姓名不符合要求！";

        /// <summary>
        /// 查询签到记录时，分页参数设置错误
        /// </summary>
        public const string PagingSizeError = "分页大小错误！";
    }
}
