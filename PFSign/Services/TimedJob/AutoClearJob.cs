﻿using Microsoft.Extensions.Logging;
using PFSignDemo.Data;
using Pomelo.AspNetCore.TimedJob;
using System;
using System.Linq;

namespace PFSignDemo.Services
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
            _logger.LogInformation("Use the AutoClearJob");
        }

        [Invoke(Begin="2018-01-01" ,Interval = 1000 * 60 * 60 * 24)]
        public void Clear(RecordDbContext context)
        {
            _logger.LogInformation("Invoke the AutoClearJob");

            // 查询未签退列表
            var unsignoutList = (from r in context.Records
                                 where r.SignOutTime == null
                                 select r).ToList();

            // 对未签退的人进行处理
            foreach (var record in unsignoutList)
            {
                // 未签退的最多记8小时
                record.SignOutTime = 
                    DateTime.UtcNow > record.SignInTime.AddHours(8)
                    ? record.SignInTime.AddHours(8) : DateTime.UtcNow;
                // 其他惩罚措施
                // ...
            }

            // 写入数据库
            try
            {
                context.UpdateRange(unsignoutList);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
    }
}
