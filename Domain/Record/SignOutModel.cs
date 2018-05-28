using PFSign.Attributes;
using PFSign.Resources;
using System.ComponentModel.DataAnnotations;

namespace PFSign.Domain.Record
{
    /// <summary>
    /// 签退Model
    /// </summary>
    public class SignOutModel
    {
        // 学号
        [Required(AllowEmptyStrings = false, ErrorMessage = RecordErrorResource.StudentIdIncorrect)]
        // 要求当前为已签到状态
        [Signed(true, ErrorMessage = RecordErrorResource.NotSigned)]
        // 学号应为8位数字
        [StringLength(8, MinimumLength = 8, ErrorMessage = RecordErrorResource.StudentIdIncorrect)]
        public string StudentId { get; set; }
    }
}
