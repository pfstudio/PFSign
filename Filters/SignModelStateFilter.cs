using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PFSign.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFSign.Filters
{
    public class SignModelStateFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                List<string> errorMsgs = new List<string>();
                foreach (var item in context.ModelState.Values)
                {
                    errorMsgs.AddRange(item.Errors.Select(error => error.ErrorMessage));
                }

                context.Result = new JsonResult(RecordResult.Fail(errorMsgs.Distinct().ToArray()));
            }
            else
            {
                var resultContext = await next();
            }
        }
    }
}
