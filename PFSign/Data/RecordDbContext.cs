using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PFSignDemo.Models;

namespace PFSignDemo.Data
{
    public class RecordDbContext:DbContext
    {
        public RecordDbContext(DbContextOptions<RecordDbContext> options)
           : base(options)
        {
        }

        public DbSet<Record> Records { get; set; }
    }
}
