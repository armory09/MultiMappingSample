using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace MultiMappingInDapper.Models
{
    [Table("Student")]
    public sealed class Student
    {
        public Student()
        {
            Enrollments = new List<Enrollment>();
        }
        [Key]
        public int StudentId { get; set; }
        public string Lastname { get; set; }
        public string FirstMidName { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public List<Enrollment> Enrollments { get; set; }
    }
}
