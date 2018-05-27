using System.ComponentModel.DataAnnotations;

namespace PFSign.Models
{
    public class User
    {
        [Key]
        public string StudentId { get; set; }
        public string Name { get; set; }
    }
}
