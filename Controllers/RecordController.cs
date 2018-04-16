using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PFStudio.PFSign.Data;
using PFStudio.PFSign.Domain;
using PFStudio.PFSign.Models;
using PFStudio.PFSign.Resources;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PFStudio.PFSign.Controllers
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
            var records = 
                await model.Filter(_context.Records.AsNoTracking())
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
        public async Task<RecordResult> SignIn([FromBody]SignInModel model)
        {
            // 检查是否允许签到
            RecordResult checkResult = await model.Check(_context.Records.AsNoTracking());
            if (!checkResult.Result)
            {
                return checkResult;
            }

            // 创建签到记录
            Record record = model.Create();

            // 签到
            record.SignIn();

            // 将记录添加到数据库
            if (!await SaveRecordAsync(record))
            {
                return RecordResult.Fail(RecordErrorResource.DataBaseError);
            }

            return RecordResult.Success();
        }

        /// <summary>签退Api</summary>
        /// <returns>签到结果<see cref="RecordResult"/></returns>
        [HttpPost("[Action]")]
        public async Task<RecordResult> SignOut([FromBody]SignOutModel model)
        {
            // 检查签退的相关参数和状态
            RecordResult checkResult = await model.Check(_context.Records.AsNoTracking());
            if (!checkResult.Result)
            {
                return checkResult;
            }

            // 从数据库获取签到记录
            Record record = await (from r in _context.Records
                                   where r.SignOutTime == null
                                   && r.StudentId == model.StudentId
                                   select r).FirstOrDefaultAsync();
            if (record == null)
            {
                return RecordResult.Fail(RecordErrorResource.RecordError);
            }

            // 签退
            record.SignOut();

            // 更新签到记录
            if (!await SaveRecordAsync(record))
            {
                return RecordResult.Fail(RecordErrorResource.DataBaseError);
            }

            return RecordResult.Success();
        }

        private async Task<bool> SaveRecordAsync(Record record)
        {
            try
            {
                _context.Update(record);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"DataBase Error: {e.Message}");
                return false;
            }

            return true;
        }
    }
}