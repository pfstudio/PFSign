using PFSign.Resources;
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
        // 学号应为8位数字
        [StringLength(8, MinimumLength = 8, ErrorMessage = RecordErrorResource.StudentIdIncorrect)]
        public string StudentId { get; set; }
        // 跳过页数不为负
        // 默认不跳过
        [Range(0, int.MaxValue, ErrorMessage = RecordErrorResource.PagingSizeError)]
        public int Skip { get; set; } = 0;
        // 一次最多获取100条记录
        // 默认取20条记录
        [Range(0, 100, ErrorMessage = RecordErrorResource.PagingSizeError)]
        public int Size { get; set; } = 20;
    }
}
