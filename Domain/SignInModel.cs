using PFSign.Attributes;
using PFSign.Models;
using PFSign.Resources;
using System.ComponentModel.DataAnnotations;

namespace PFSign.Domain
{
    /// <summary>
    /// 签到Model
    /// </summary>
    public class SignInModel
    {
        // 学号
        [Required(AllowEmptyStrings = false, ErrorMessage = RecordErrorResource.UserInfoIncorrect)]
        [Signed(false, ErrorMessage = RecordErrorResource.StudentSigned)]
        public string StudentId { get; set; }
        // 姓名
        [Required(AllowEmptyStrings = false, ErrorMessage = RecordErrorResource.UserInfoIncorrect)]
        public string Name { get; set; }

        public Record Create() => new Record(StudentId, Name);
    }
}
