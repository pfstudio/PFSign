using Microsoft.EntityFrameworkCore;
using PFSign.Models;

namespace PFSign.Data
{
    /// <summary>
    /// 签到记录的数据库上下文
    /// </summary>
    public class RecordDbContext : DbContext
    {
        public RecordDbContext(DbContextOptions<RecordDbContext> options)
           : base(options)
        {
        }

        public DbSet<Record> Records { get; set; }
    }
}
