namespace PFStudio.PFSign.Domain
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

        /// <summary>
        /// 签到/签退成功，且不带说明信息
        /// </summary>
        /// <returns></returns>
        public static RecordResult Success()
        {
            return new RecordResult()
            {
                Result = true
            };
        }

        /// <summary>
        /// 签到/签退成功
        /// </summary>
        /// <param name="message">额外的说明信息</param>
        /// <returns></returns>
        public static RecordResult Success(string message)
        {
            return new RecordResult()
            {
                Result = true,
                Message = message
            };
        }

        /// <summary>
        /// 签到/签退失败
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <returns></returns>
        public static RecordResult Fail(string error)
        {
            return new RecordResult()
            {
                Result = false,
                Message = error
            };
        }
    }
}
