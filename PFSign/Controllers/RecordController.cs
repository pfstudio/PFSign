using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PFSignDemo.Data;
using PFSignDemo.Models;
using PFSignDemo.Resources;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PFSignDemo.Controllers
{
    [Route("/api/[Controller]")]
    [Authorize]
    public class RecordController
    {
        private readonly RecordDbContext _context;
        private readonly ILogger _logger;

        public RecordController(
            RecordDbContext context,
            ILogger<RecordController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>查询Api</summary>
        /// <param name="begin">开始日期</param>
        /// <param name="end">结束日期</param>
        /// <returns>[{Name, Seat, SignInTime, SignOutTime}]</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<object> Index
            (DateTime? begin, DateTime? end)
        {
            // 默认开始时间为当天
            DateTime beginTime = begin ?? DateTime.Today.ToUniversalTime();
            // 默认结束时间为开始时间的一天
            DateTime endTime = end ?? beginTime.AddDays(1);
            // 禁用Tracking以提高性能
            // TODO: 返回Local时间
            return await (from r in _context.Records.AsNoTracking()
                          where r.SignInTime >= beginTime
                          && r.SignInTime <= endTime
                          select new
                          {
                              Name        = r.Name,
                              Seat        = r.Seat,
                              SignInTime  = r.SignInTime,
                              SignOutTime = r.SignOutTime
                          }).ToListAsync();
        }

        /// <summary>签到Api</summary>
        /// <param name="model"><seealso cref="SignInModel"/></param>
        /// <returns>签到结果<see cref="RecordResult"/></returns>
        [HttpPost("[Action]")]
        public async Task<RecordResult> SignIn(SignInModel model)
        {
            // 检查是否允许签到
            RecordResult checkResult = await model.Check(_context.Records.AsNoTracking());
            if (!checkResult.Result)
            {
                return checkResult;
            }

            // 创建签到记录
            Record record = Record.Create(model);

            // 将记录添加到数据库
            // TODO: 需要更细致的错误处理
            try
            {
                _logger.LogDebug(
                    $"{record.Id} -- {record.Name}/{record.StudentId}" +
                    $" Sign In at {record.SignInTime} with Seat {record.Seat}");
                _context.Add(record);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RecordResult.Fail($"数据库错误:{e}");
            }

            return RecordResult.Success();
        }

        /// <summary>签退Api</summary>
        /// <param name="model"><seealso cref="SignOutModel"/></param>
        /// <returns>签到结果<see cref="RecordResult"/></returns>
        [HttpPost("[Action]")]
        public async Task<RecordResult> SignOut(SignOutModel model)
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

            // 记录当前时间为签退时间
            record.SignOutTime = DateTime.UtcNow;

            // 更新签到记录
            // TODO: 需要更细致的错误处理
            try
            {
                _logger.LogDebug(
                    $"{record.Id} -- {record.Name}/{record.StudentId}" +
                    $" Sign Out at {record.SignOutTime} with Seat {record.Seat}");
                _context.Update(record);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RecordResult.Fail($"数据库错误:{e}");
            }

            return RecordResult.Success();
        }
    }
}