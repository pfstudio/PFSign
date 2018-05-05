using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PFSign.Data;
using PFSign.Domain;
using PFSign.Extensions;
using PFSign.Filters;
using PFSign.Models;
using PFSign.Resources;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PFSign.Controllers
{
    [Route("/Record")]
    public class RecordController : Controller
    {
        // 签到记录的上下文
        private readonly RecordDbContext _context;
        // 用于输出日志
        private readonly ILogger _logger;

        public RecordController(
            RecordDbContext context, ILogger<RecordController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>查询Api</summary>
        /// <returns>[{Name, Seat, SignInTime, SignOutTime}]</returns>
        [HttpGet("[action]")]
        public async Task<JsonResult> Query(QueryModel model,
            [FromServices]JsonSerializerSettings serializerSettings)
        {
            // 禁用Tracking以提高性能
            var records =  await _context.Records.AsNoTracking()
                .Filter(model)
                .Select(r => new
                {
                    r.Name,
                    r.SignInTime,
                    r.SignOutTime
                }).ToListAsync();

            // 在解析时，把时间转换为当地时间
            return new JsonResult(records, serializerSettings);
        }

        /// <summary>签到Api</summary>
        /// <returns>签到结果<see cref="RecordResult"/></returns>
        [HttpPost("[Action]")]
        [TypeFilter(typeof(SignModelStateFilter))]
        public async Task<RecordResult> SignIn([FromBody]SignInModel model)
        {
            // 创建签到记录
            Record record = model.Create();

            // 签到
            record.SignIn();

            // 将记录添加到数据库
            try
            {
                _context.Add(record);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"DataBase Error: {e.Message}");
                return RecordResult.Fail(RecordErrorResource.DataBaseError);
            }


            return RecordResult.Success();
        }

        /// <summary>签退Api</summary>
        /// <returns>签到结果<see cref="RecordResult"/></returns>
        [HttpPost("[Action]")]
        [TypeFilter(typeof(SignModelStateFilter))]
        public async Task<RecordResult> SignOut([FromBody]SignOutModel model)
        {
            // 从数据库获取签到记录
            Record record = await (from r in _context.Records
                                   where r.SignOutTime == null
                                   && r.StudentId == model.StudentId
                                   select r).FirstAsync();

            // 签退
            record.SignOut();

            // 更新签到记录
            try
            {
                _context.Update(record);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"DataBase Error: {e.Message}");
                return RecordResult.Fail(RecordErrorResource.DataBaseError);
            }

            return RecordResult.Success();
        }
    }
}