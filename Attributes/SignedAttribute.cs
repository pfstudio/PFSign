using PFSign.Repositorys;
using System;
using System.ComponentModel.DataAnnotations;

namespace PFSign.Attributes
{
    /// <summary>
    /// 查询数据库判断签到状态
    /// </summary>
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
            // 获取仓储对象
            IRecordRepository recordRepository = 
                (IRecordRepository)validationContext.GetService(typeof(IRecordRepository));
            // 判断状态
            return _requriedState == recordRepository.IsSigned(studentId) ?
                ValidationResult.Success :
                // 返回错误信息
                new ValidationResult(FormatErrorMessage(""));
        }
    }
}
