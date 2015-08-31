using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace MultiMappingInDapper.Models
{
    [Table("Course")]
    public class Course
    {    

        public int Id { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        [Write(false)]
        public virtual IEnumerable<Enrollment> Enrollments { get; set; }
    }
}
