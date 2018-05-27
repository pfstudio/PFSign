using Microsoft.EntityFrameworkCore;
using PFSign.Data;
using PFSign.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFSign.Repositorys
{
    public class EFRecordRepository : IRecordRepository
    {
        private RecordDbContext _dbContext;

        public EFRecordRepository(RecordDbContext context)
        {
            _dbContext = context;
        }

        public bool IsSigned(string studentId)
        {
            return (from r in _dbContext.Records.AsNoTracking()
                    where r.SignOutTime == null
                    && r.StudentId == studentId
                    select r).Count() > 0;
        }

        public async Task ClearAllUnSignOutAsync()
        {
            // 查找所有未签到的人员
            var records = await (from r in _dbContext.Records.AsNoTracking()
                                 where r.SignOutTime == null
                                 select r).ToListAsync();
            // 为所有人签退
            records.ForEach(r => r.SignOut());

            // 更新数据库
            _dbContext.UpdateRange(records);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Record>> QueryAsync(DateTime begin, DateTime end, string studentId, int skip, int size)
        {
            // 筛选时间
            var query = _dbContext.Records.Where(r => r.SignInTime >= begin && r.SignInTime <= end);

            // 筛选学号
            if (!string.IsNullOrEmpty(studentId))
            {
                query = query.Where(r => r.StudentId == studentId);
            }

            // 以签到时间升序
            query = query.OrderBy(r => r.SignInTime);
            // 启用分页
            query = query.Skip(skip).Take(size);

            return await query.ToListAsync();
        }

        public async Task SignInAsync(string studentId, string name)
        {
            // 创建签到
            Record record = Record.SignIn(studentId, name);

            // 保存到数据库
            _dbContext.Add(record);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SignOutAsync(string studentId)
        {
            // 查找签到记录
            Record record = await (from r in _dbContext.Records
                                   where r.StudentId == studentId
                                   && r.SignOutTime == null
                                   select r).FirstAsync();
            // 签退
            record.SignOut();
            
            // 保存到数据库
            _dbContext.Update(record);
            await _dbContext.SaveChangesAsync();
        }
    }
}
