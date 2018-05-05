using System;

namespace PFSign.Domain
{
    /// <summary>
    /// 查询Model
    /// </summary>
    public class QueryModel
    {
        public DateTime? Begin { get; set; }
        public DateTime? End { get; set; }
        public string StudentId { get; set; }
        public int Skip { get; set; } = 0;
        public int Size { get; set; } = 20;
    }
}
