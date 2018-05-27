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
        [Required(AllowEmptyStrings = false, ErrorMessage = RecordErrorResource.UserInfoIncorrect)]
        // 要求当前为非签到状态
        [Signed(false, ErrorMessage = RecordErrorResource.StudentSigned)]
        public string StudentId { get; set; }
        // 姓名
        [Required(AllowEmptyStrings = false, ErrorMessage = RecordErrorResource.UserInfoIncorrect)]
        public string Name { get; set; }
    }
}
