using PFSign.Attributes;
using PFSign.Resources;
using System.ComponentModel.DataAnnotations;

namespace PFSign.Domain
{
    /// <summary>
    /// 签退Model
    /// </summary>
    public class SignOutModel
    {
        // 学号
        [Required(AllowEmptyStrings = false, ErrorMessage = RecordErrorResource.UserInfoIncorrect)]
        [Signed(true, ErrorMessage = RecordErrorResource.RecordError)]
        public string StudentId { get; set; }
    }
}
