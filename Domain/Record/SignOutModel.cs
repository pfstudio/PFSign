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
        [Required(AllowEmptyStrings = false, ErrorMessage = RecordErrorResource.UserInfoIncorrect)]
        // 要求当前为已签到状态
        [Signed(true, ErrorMessage = RecordErrorResource.RecordError)]
        public string StudentId { get; set; }
    }
}
