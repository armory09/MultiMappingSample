using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace MultiMappingInDapper.Models
{
    [Table("Student")]
    public class Student
    {    
        public int Id { get; set; }
        public string Lastname { get; set; }
        public string FirstMidName { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        [Write(false)]
        public virtual IEnumerable<Enrollment> Enrollments { get; set; }
    }
}
