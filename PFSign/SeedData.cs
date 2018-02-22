using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PFStudio.PFSign.Data;
using System;

namespace PFStudio.PFSign
{
    public class SeedData
    {
        /// <summary>
        /// 确保数据库迁移完成
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void EnsureMigrate(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Migrating database...");

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<RecordDbContext>().Database.Migrate();
            }

            Console.WriteLine("Done migrating database.");
            Console.WriteLine();
        }
    }
}
