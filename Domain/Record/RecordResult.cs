namespace PFSign.Domain.Record
{
    /// <summary>
    /// 用于返回签到/签退结果的Model
    /// </summary>
    public class RecordResult
    {
        /// <summary>
        /// 签到/签退结果
        /// </summary>
        public bool Result { get; private set; }
        /// <summary>
        /// 额外的说明信息
        /// </summary>
        public string Message { get; private set; }

        private RecordResult() { }

        // 参考ValidResult改为静态字段
        /// <summary>
        /// 签到/签退成功，且不带说明信息
        /// </summary>
        /// <returns></returns>
        public static RecordResult Success = new RecordResult { Result = true };

        /// <summary>
        /// 签到/签退失败
        /// </summary>
        /// <param name="errors">错误信息</param>
        /// <returns></returns>
        public static RecordResult Fail(params string[] errors)
        {
            return new RecordResult()
            {
                Result = false,
                Message = string.Join("|", errors)
            };
        }
    }
}
