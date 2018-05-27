using System;
using System.ComponentModel.DataAnnotations;

namespace PFSign.Models
{
    public class DateDuration
    {
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
