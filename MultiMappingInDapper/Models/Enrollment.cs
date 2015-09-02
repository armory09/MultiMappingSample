using Dapper.Contrib.Extensions;

namespace MultiMappingInDapper.Models
{
    [Table("Enrollment")]
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public string Grade { get; set; }

        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }
    }
}
