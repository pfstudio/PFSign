using Microsoft.EntityFrameworkCore;
using PFSignDemo.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFSignDemo.Models
{
    public class SignOutModel
    {
        public string StudentId { get; set; }

        public async Task<RecordResult> Check(IQueryable<Record> records)
        {
            if (StudentId == null)
            {
                return RecordResult.Fail(RecordErrorResource.UserInfoIncorrect);
            }

            if (await (from r in records
                       where r.SignOutTime == null
                       && r.StudentId == StudentId
                       select r).CountAsync() == 0)
            {
                return RecordResult.Fail(RecordErrorResource.RecordError);
            }

            return RecordResult.Success();
        }
    }
}
