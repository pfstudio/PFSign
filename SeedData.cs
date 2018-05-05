using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PFSign.Data;
using System;
using System.Threading;

namespace PFSign
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
                RecordDbContext context = scope.ServiceProvider.GetRequiredService<RecordDbContext>();
                bool retry = true;
                int count = 0;
                int maxRetry = 5;
                do
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Stop 1s, wait database start...");

                    try
                    {
                        context.Database.EnsureCreated();
                        retry = false;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Connect Failed...Retry {count}");
                        count++;
                        if (count > maxRetry)
                        {
                            Console.WriteLine("Try too many times");
                            throw;
                        }
                    }
                } while (retry);
            }

            Console.WriteLine("Done migrating database.");
            Console.WriteLine();
        }
    }
}
