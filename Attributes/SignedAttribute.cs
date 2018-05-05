using Microsoft.EntityFrameworkCore;
using PFSign.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PFSign.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SignedAttribute : ValidationAttribute
    {
        private bool _requriedState;

        public SignedAttribute(bool requriedState)
        {
            _requriedState = requriedState;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // 获得学号
            string studentId = value as string;
            // 获取数据库上下文
            RecordDbContext dbContext = 
                (RecordDbContext)validationContext.GetService(typeof(RecordDbContext));
            // 确认签到状态
            bool state = (from r in dbContext.Records.AsNoTracking()
                          where r.SignOutTime == null
                          && r.StudentId == studentId
                          select r).Count() > 0;
            // 判断状态
            return _requriedState == state ?
                ValidationResult.Success :
                new ValidationResult(FormatErrorMessage(""));
        }
    }
}
