using System;
using System.ComponentModel.DataAnnotations;

namespace PFSign.Domain.Record
{
    /// <summary>
    /// 查询Model
    /// </summary>
    public class QueryModel
    {
        public DateTime? Begin { get; set; }
        public DateTime? End { get; set; }
        public string StudentId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = Resources.RecordErrorResource.PagingSizeError)]
        public int Skip { get; set; } = 0;
        [Range(0, 100, ErrorMessage = Resources.RecordErrorResource.PagingSizeError)]
        public int Size { get; set; } = 20;
    }
}
