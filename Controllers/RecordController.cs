using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PFSign.Domain.Record;
using PFSign.Filters;
using PFSign.Repositorys;
using PFSign.Resources;
using System;
using System.Threading.Tasks;

namespace PFSign.Controllers
{
    [Route("/api/Record")]
    // 调用Filter，检验对应数据注解中的要求
    [TypeFilter(typeof(RecordModelStateFilter))]
    public class RecordController : Controller
    {
        // 签到的仓储对象
        private readonly IRecordRepository _recordRepository;
        // 用于输出日志
        private readonly ILogger _logger;

        public RecordController(
            IRecordRepository recordRepository, ILogger<RecordController> logger)
        {
            _recordRepository = recordRepository;
            _logger = logger;
        }

        /// <summary>查询Api</summary>
        /// <returns>[{Name, SignInTime, SignOutTime}]</returns>
        [HttpGet]
        public async Task<JsonResult> Query(QueryModel model,
            [FromServices]JsonSerializerSettings serializerSettings)
        {
            // 默认开始时间为当天
            DateTime begin = model.Begin?.ToUniversalTime() ?? DateTime.Today.ToUniversalTime();
            // 默认结束时间为开始时间后的一天
            DateTime end = model.End?.ToUniversalTime() ?? begin.AddDays(1);
            // 查询
            var records = await _recordRepository.QueryAsync(
                begin, end, model.StudentId, model.Skip, model.Size);

            // 在解析时，把时间转换为当地时间
            return new JsonResult(
                new
                {
                    Total = await _recordRepository.CountAsync(begin, end, model.StudentId),
                    Result = records
                }, serializerSettings);
        }

        /// <summary>签到Api</summary>
        /// <returns>签到结果<see cref="RecordResult"/></returns>
        [HttpPost("[Action]")]
        public async Task<RecordResult> SignIn([FromBody]SignInModel model)
        {
            // 签到
            try
            {
                await _recordRepository.SignInAsync(model.StudentId, model.Name);
            }
            catch (Exception e)
            {
                _logger.LogError($"DataBase Error: {e.Message}");
                return RecordResult.Fail(RecordErrorResource.DataBaseError);
            }


            return RecordResult.Success;
        }

        /// <summary>签退Api</summary>
        /// <returns>签到结果<see cref="RecordResult"/></returns>
        [HttpPost("[Action]")]
        public async Task<RecordResult> SignOut([FromBody]SignOutModel model)
        {
            // 签退
            try
            {
                await _recordRepository.SignOutAsync(model.StudentId);
            }
            catch (Exception e)
            {
                _logger.LogError($"DataBase Error: {e.Message}");
                return RecordResult.Fail(RecordErrorResource.DataBaseError);
            }

            return RecordResult.Success;
        }

        [HttpDelete("{id}")]
        public async Task<RecordResult> DeleteRecord([FromRoute]int id)
        {
            // 删除
            try
            {
                await _recordRepository.DeleteWithAsync(id);
            }
            catch (Exception e)
            {
                _logger.LogError($"DataBase Error: {e.Message}");
                return RecordResult.Fail(RecordErrorResource.RecordError);
            }

            return RecordResult.Success;
        }
    }
}