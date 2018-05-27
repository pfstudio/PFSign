using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PFSign.Domain.Record;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFSign.Filters
{
    /// <summary>
    /// Filter与属性注解结合用于确保签到状态
    /// </summary>
    public class SignModelStateFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            // 若没有通过模型状态的检验，
            // 则返回RecordResult.Fail(errors)
            if (!context.ModelState.IsValid)
            {
                List<string> errorMsgs = new List<string>();
                foreach (var item in context.ModelState.Values)
                {
                    errorMsgs.AddRange(item.Errors.Select(error => error.ErrorMessage));
                }

                // 错误去重
                context.Result = new JsonResult(RecordResult.Fail(errorMsgs.Distinct().ToArray()));
            }
            else
            {
                // 通过检验后异步调用之后的步骤
                var resultContext = await next();
            }
        }
    }
}
