using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace MultiMappingInDapper.Models
{
    [Table("Student")]
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public string Lastname { get; set; }
        public string FirstMidName { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public virtual IEnumerable<Enrollment> Enrollments { get; set; }
    }
}
