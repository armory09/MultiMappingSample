using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace MultiMappingInDapper.Models
{
    [Table("Course")]
    public sealed class Course
    {
        public Course()
        {
            Enrollments = new List<Enrollment>();
        }

        [Key]
        public int CourseId { get; set; }
        public string Title { get; set; }
        public int? Credits { get; set; }
        public List<Enrollment> Enrollments { get; set; }
    }
}
