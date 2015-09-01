using System.Collections;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace MultiMappingInDapper.Models
{
    [Table("Course")]
    public class Course
    {   [Key]
        public int CourseId { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public virtual IEnumerable<Enrollment> Enrollments { get; set; }
        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}
