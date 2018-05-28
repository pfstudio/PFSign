using PFSign.Attributes;
using PFSign.Resources;
using System.ComponentModel.DataAnnotations;

namespace PFSign.Domain.Record
{
    /// <summary>
    /// 签到Model
    /// </summary>
    public class SignInModel
    {
        // 学号
        [Required(AllowEmptyStrings = false, ErrorMessage = RecordErrorResource.StudentIdIncorrect)]
        // 要求当前为非签到状态
        [Signed(false, ErrorMessage = RecordErrorResource.HasSigned)]
        // 学号应为8位数字
        [StringLength(8, MinimumLength = 8, ErrorMessage = RecordErrorResource.StudentIdIncorrect)]
        public string StudentId { get; set; }
        // 姓名
        [Required(AllowEmptyStrings = false, ErrorMessage = RecordErrorResource.NameIncorrect)]
        public string Name { get; set; }
    }
}
