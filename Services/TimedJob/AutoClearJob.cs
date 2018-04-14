﻿using Microsoft.Extensions.Logging;
using PFStudio.PFSign.Data;
using Pomelo.AspNetCore.TimedJob;
using System;
using System.Linq;

namespace PFStudio.PFSign.Services
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
                // 对签到时间超过24小时的人，只记录8小时的签到时间
                if (record.SignInTime.AddDays(1) > DateTime.UtcNow)
                {
                    continue;
                }

                record.SignOutWithTimeOut();
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