using Microsoft.Extensions.Logging;
using PFSign.Data;
using Pomelo.AspNetCore.TimedJob;
using System;
using System.Linq;

namespace PFSign.Services
{
    /// <summary>
    /// 自动清人
    /// </summary>
    public class AutoClearJob : Job
    {
        private readonly ILogger _logger;

        public AutoClearJob(ILogger<AutoClearJob> logger)
        {
            _logger = logger;
            _logger.LogDebug("Use the AutoClearJob");
        }

        // 每天定时清人
        [Invoke(Begin = "2018-01-01", Interval = 1000 * 60 * 60 * 24)]
        // 注入数据库上下文
        public void Clear(RecordDbContext context)
        {
            _logger.LogDebug("Invoke the AutoClearJob");

            // 查询未签退列表
            var unsignOutList = (from r in context.Records
                                 where r.SignOutTime == null
                                 select r).ToList();

            // 对未签退的人进行处理
            foreach (var record in unsignOutList)
            {
                if (record.IsTimeOut())
                {
                    record.SignOutWithTimeOut();
                }
                else
                {
                    record.SignOut();
                }
            }

            // 写入数据库
            try
            {
                context.UpdateRange(unsignOutList);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
    }
}
